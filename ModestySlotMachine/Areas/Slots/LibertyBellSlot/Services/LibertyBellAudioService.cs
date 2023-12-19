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
        private const string CurrentTrackSettingsKey = LibertyBellConstants.GameSettingsBetKey + "CurrentTrack";
        private const string CurrentTrackTimeSettingsKey = LibertyBellConstants.GameSettingsBetKey + "CurrentTrackTime";

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

            userData.GameRelatedSettings.TryGetGuid(CurrentTrackSettingsKey, out Guid trackId);
            userData.GameRelatedSettings.TryGetDouble(CurrentTrackTimeSettingsKey, out double trackTime);

            _backgroundMusicPlayer.Playlist = PlaylistBuilder.Create()
                .StartWith(trackId)
                .Next(new Track
                {
                    Id = Guid.Parse("00c53c45-d1a4-4eb2-a45f-d802d90fa007"),
                    Name = "SergeQuadrado - Little Adventure",
                    AudioStream = new MemoryStream(BackgroundTracks.ResourceManager.GetObject("little_adventure_95822") as byte[]),
                })
                .Next(new Track
                {
                    Id = Guid.Parse("454b7df6-0c77-4ade-925a-06dfed76c40d"),
                    Name = "PianoAmor - In The Saloon",
                    AudioStream = new MemoryStream(BackgroundTracks.ResourceManager.GetObject("in_the_saloon_116225") as byte[]),
                })
                .Next(new Track
                {
                    Id = Guid.Parse("2fe6df2e-2927-4550-9cc4-18aec65e5629"),
                    Name = "JuliusH - Cowboy's Sundown (Country Ballad)",
                    AudioStream = new MemoryStream(BackgroundTracks.ResourceManager.GetObject("cowboy39s_sundown_country_ballad_623") as byte[]),
                })
                .Next(new Track
                {
                    Id = Guid.Parse("380589cd-9753-439a-bd2e-90e66de75b55"),
                    Name = "Andrewfai - Cowboy sunset music",
                    AudioStream = new MemoryStream(BackgroundTracks.ResourceManager.GetObject("cowboy_sunset_music_4274") as byte[]),
                })
                .Build();

            _backgroundMusicPlayer.TrackCurrentPosition = trackTime;
            _backgroundMusicPlayer.TrackChange += OnTrackChange;
        }

        private void OnTrackChange(object sender, TrackEventArgs e)
        {
            _userDataService.GetUserDataAsync().ContinueWith(async _ => await SaveCurrentPlayerState());
        }

        public async Task SaveCurrentPlayerState()
        {
            var userData = await _userDataService.GetUserDataAsync();

            userData.GameRelatedSettings.AddOrUpdate(CurrentTrackSettingsKey, _backgroundMusicPlayer.CurrentTrack.Id);
            userData.GameRelatedSettings.AddOrUpdate(CurrentTrackTimeSettingsKey, _backgroundMusicPlayer.TrackCurrentPosition);

            await _userDataService.SaveUserDataAsync(userData);
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