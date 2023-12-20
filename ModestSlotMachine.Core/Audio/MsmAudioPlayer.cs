using ModestSlotMachine.Core.Audio.Exceptions;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModestSlotMachine.Core.Audio
{
    public class MsmAudioPlayer : AudioPlayerBase
    {
        public int _currentTrackIndex;
        private Playlist _playlist;

        private CancellationTokenSource _playNextTrackCancellationTokenSource;

        public IAudioPlayer CurrentTrackHandler { get; private set; }
        
        public Playlist Playlist
        {
            get => _playlist;
            set
            {
                _playlist = value;
                InitPlaylist();
            }
        }

        public Track CurrentTrack { get; private set; }

        public bool IsPlaying => CurrentTrackHandler?.IsPlaying ?? false;

        public double TrackCurrentPosition
        {
            get
            {
                return CurrentTrackHandler?.CurrentPosition ?? default;
            }
            set
            {
                if (CurrentTrackHandler == null || !CurrentTrackHandler.CanSeek)
                    return;

                if (_playNextTrackCancellationTokenSource != null)
                {
                    // Prevent moving to next track
                    _playNextTrackCancellationTokenSource.Cancel();
                }

                CurrentTrackHandler.Seek(value);
                TrackPositionChange?.Invoke(this, new TrackEventArgs { Index = _currentTrackIndex, Track = CurrentTrack, TrackTime = value });
            }
        }

        #region Events
        public event EventHandler TrackPlay;
        public event EventHandler TrackPause;
        public event EventHandler<TrackEventArgs> TrackChange;
        public event EventHandler<TrackEventArgs> TrackPositionChange;
        public event EventHandler TrackPlaybackEnd;
        public event EventHandler PlaylistSet;
        #endregion

        public MsmAudioPlayer(IAudioManager audioManager) : base(audioManager) { }

        private void InitPlaylist()
        {
            _currentTrackIndex = 0;
            CurrentTrack = Playlist.Tracks[_currentTrackIndex];
            CurrentTrackHandler = CreatePlayer(CurrentTrack.AudioStream);
            CurrentTrackHandler.PlaybackEnded += OnPlaybackEnded;

            PlaylistSet?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="PlaylistNotSetException"></exception>
        public void Play()
        {
            if (Playlist == null)
                throw new PlaylistNotSetException();

            if (CurrentTrackHandler.IsPlaying)
                return;

            CurrentTrackHandler.Volume = Volume;
            TrackPlay?.Invoke(this, EventArgs.Empty);
            CurrentTrackHandler.Play();
        }

        private void OnPlaybackEnded(object? sender, EventArgs e)
        {
            _playNextTrackCancellationTokenSource = new CancellationTokenSource();

            Task.Delay(3000, _playNextTrackCancellationTokenSource.Token).ContinueWith(t => 
                { 
                    if (!t.IsCanceled) 
                        PlayNextTrack();
                }, 
                _playNextTrackCancellationTokenSource.Token);

            TrackPlaybackEnd?.Invoke(this, EventArgs.Empty);
        }

        public void Pause()
        {
            CurrentTrackHandler?.Pause();
            TrackPause?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="PlaylistNotSetException"></exception>
        public void PlayNextTrack()
        {
            if (Playlist == null)
                throw new PlaylistNotSetException();

            int nextTrackIndex = _currentTrackIndex + 1;
            if (nextTrackIndex >= Playlist.Tracks.Count)
                nextTrackIndex = 0;

            InitTrackByIndex(nextTrackIndex);
            TrackChange?.Invoke(this, new TrackEventArgs { Track = CurrentTrack, Index = nextTrackIndex, TrackTime = CurrentTrackHandler.CurrentPosition });
            Play();
        }

        /// <summary>
        /// </summary>
        /// <exception cref="PlaylistNotSetException"></exception>
        public void PlayPreviousTrack()
        {
            if (Playlist == null)
                throw new PlaylistNotSetException();

            int nextTrackIndex = _currentTrackIndex - 1;
            if (nextTrackIndex < 0)
                nextTrackIndex = Playlist.Tracks.Count - 1;
            
            InitTrackByIndex(nextTrackIndex);
            TrackChange?.Invoke(this, new TrackEventArgs { Track = CurrentTrack, Index = nextTrackIndex, TrackTime = CurrentTrackHandler.CurrentPosition });
            Play();
        }

        private void InitTrackByIndex(int nextTrackIndex)
        {
            try
            {
                CurrentTrackHandler.PlaybackEnded -= OnPlaybackEnded;
                CurrentTrackHandler.Stop();

                CurrentTrack.AudioStream.Position = 0;
                var newStream = new MemoryStream();
                CurrentTrack.AudioStream.CopyTo(newStream);
                CurrentTrack.AudioStream = newStream;

                var handlerRef = CurrentTrackHandler;
                CurrentTrackHandler = null!;
                handlerRef.Dispose();

                _currentTrackIndex = nextTrackIndex;
                CurrentTrack = Playlist.Tracks[nextTrackIndex];
                CurrentTrackHandler = CreatePlayer(CurrentTrack.AudioStream);
                CurrentTrackHandler.PlaybackEnded += OnPlaybackEnded;
            }
            catch (Exception ex)
            {
                // COM exception can occur when CurrentTrackHandler.Stop called too soon after it was created
            }
        }
    }
}
