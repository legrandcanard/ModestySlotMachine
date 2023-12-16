using Microsoft.Extensions.Logging;
using ModestySlotMachine.Areas.Slots.LibertyBellSlot.Resources;
using ModestySlotMachine.Areas.Slots.LibertyBellSlot.Util;
using ModestySlotMachine.Core.Audio;
using ModestySlotMachine.Core.Entities;
using ModestySlotMachine.Core.Services;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace ModestySlotMachine.Areas.Slots.LibertyBellSlot.Services
{
    public class LibertyBellAudioService : AudioPlayerBase, IDisposable
    {
        private const string CurrentTrackSettingsKey = LibertyBellContants.GameSettingsBetKey + "CurrentTrack";

        private readonly IAudioManager _audioManager;
        private readonly MsmAudioPlayer _backgroundMusicPlayer;
        private readonly UserDataService _userDataService;

        public double MusicVolume { get; set; } = 0.5;

        //Cached players
        private IAudioPlayer _fxReelSpinSoundAudioPlayer;

        public LibertyBellAudioService(IAudioManager audioManager, MsmAudioPlayer backgroundMusicPlayer, UserDataService userDataService)
            : base(audioManager)
        {
            _audioManager = audioManager;
            _userDataService = userDataService;
            _backgroundMusicPlayer = backgroundMusicPlayer;
            InitBackgroundMusicPlayerAsync();
        }

        private async Task InitBackgroundMusicPlayerAsync()
        {
            var userData = await _userDataService.GetUserDataAsync();

            userData.GameRelatedSettings.TryGetInteger(CurrentTrackSettingsKey, out int trackIndex);

            _backgroundMusicPlayer.Playlist = PlaylistBuilder.Create()
                .StartWith(trackIndex)
                .Next(new Track
                {
                    Name = "little_adventure_95822",
                    AudioStream = new MemoryStream(BackgroundTracks.ResourceManager.GetObject("little_adventure_95822") as byte[]),
                })
                .Next(new Track
                {
                    Name = "in_the_saloon_116225",
                    AudioStream = new MemoryStream(BackgroundTracks.ResourceManager.GetObject("in_the_saloon_116225") as byte[]),
                })
                .Next(new Track
                {
                    Name = "cowboy39s_sundown_country_ballad_623",
                    AudioStream = new MemoryStream(BackgroundTracks.ResourceManager.GetObject("cowboy39s_sundown_country_ballad_623") as byte[]),
                })
                .Next(new Track
                {
                    Name = "cowboy_sunset_music_4274",
                    AudioStream = new MemoryStream(BackgroundTracks.ResourceManager.GetObject("cowboy_sunset_music_4274") as byte[]),
                })
                .Build();

            _backgroundMusicPlayer.TrackChange += OnTrackChange;
        }

        private void OnTrackChange(object sender, TrackEventArgs e)
        {
            _userDataService.GetUserDataAsync().ContinueWith(async t =>
            {
                var userData = t.Result;

                userData.GameRelatedSettings.AddOrUpdate(CurrentTrackSettingsKey, e.Index);

                await _userDataService.SaveUserDataAsync(userData);
            });
        }

        public void StopAll()
        {
            StopBackgroundMusic();
            StopReelSpinSound();
        }

        public void PlayRegularWinSound()
        {
            Play(FxSounds.fairy_arcade_sparkle);
        }

        public void PlayBigWinSound()
        {
            Play(FxSounds.fairy_arcade_sparkle);
        }

        public void PlayConisFallSound()
        {
            Play(FxSounds.clinking_coins);
        }

        public void PlayReelSpinSound()
        {
            if (_fxReelSpinSoundAudioPlayer == null)
            {
                _fxReelSpinSoundAudioPlayer = _audioManager.CreatePlayer(FxSounds.reel_spin_fx);
                _fxReelSpinSoundAudioPlayer.Volume = Volume;
                _fxReelSpinSoundAudioPlayer.Loop = true;
            }
            _fxReelSpinSoundAudioPlayer.Play();
        }

        public void StopReelSpinSound()
        {
            _fxReelSpinSoundAudioPlayer?.Stop();
        }

        public void PlayReelStopSound()
        {
            Play(FxSounds.reel_stop_fx);
        }

        public void PlayBackgroundMusic()
        {
            _backgroundMusicPlayer.Volume = MusicVolume;
            _backgroundMusicPlayer.Play();
        }

        public void StopBackgroundMusic()
        {
            _backgroundMusicPlayer?.Pause();
        }

        public void Dispose()
        {
            StopAll();
        }
    }
}