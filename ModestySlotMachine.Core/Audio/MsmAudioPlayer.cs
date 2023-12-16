using ModestySlotMachine.Core.Audio.Exceptions;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModestySlotMachine.Core.Audio
{
    public class MsmAudioPlayer : AudioPlayerBase
    {
        public int _currentTrackIndex;
        private Playlist _playlist;

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

        public bool IsPlaying { get; private set; }

        object _currentTrackHandlerLock = new object();
        public double TrackCurrentPosition
        {
            get
            {
                lock (_currentTrackHandlerLock)
                {
                    return CurrentTrackHandler.CurrentPosition;
                }
            }
        }

        public event EventHandler TrackPlay;
        public event EventHandler TrackPause;
        public event EventHandler<TrackEventArgs> TrackChange;

        public MsmAudioPlayer(IAudioManager audioManager) : base(audioManager) { }

        private void InitPlaylist()
        {
            _currentTrackIndex = 0;
            CurrentTrack = Playlist.Tracks[_currentTrackIndex];
            CurrentTrackHandler = CreatePlayer(CurrentTrack.AudioStream);
            CurrentTrackHandler.PlaybackEnded += OnPlaybackEnded;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="PlaylistNotSetException"></exception>
        public void Play()
        {
            if (Playlist == null)
                throw new PlaylistNotSetException();

            CurrentTrackHandler.Volume = Volume;
            TrackPlay?.Invoke(this, EventArgs.Empty);
            CurrentTrackHandler.Play();
            IsPlaying = true;
        }

        private void OnPlaybackEnded(object? sender, EventArgs e)
        {
            Task.Delay(3000).ContinueWith(_ => PlayNextTrack());
        }

        public void Pause()
        {
            CurrentTrackHandler?.Pause();
            IsPlaying = false;
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
            TrackChange?.Invoke(this, new TrackEventArgs { Track = CurrentTrack, Index = nextTrackIndex });
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
            Play();
        }

        private void InitTrackByIndex(int nextTrackIndex)
        {
            lock (_currentTrackHandlerLock)
            {
                try
                {
                    CurrentTrackHandler.PlaybackEnded -= OnPlaybackEnded;
                    CurrentTrackHandler.Stop();

                    CurrentTrack.AudioStream.Position = 0;
                    var newStream = new MemoryStream();
                    CurrentTrack.AudioStream.CopyTo(newStream);
                    CurrentTrack.AudioStream = newStream;

                    CurrentTrackHandler.Dispose();

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
}
