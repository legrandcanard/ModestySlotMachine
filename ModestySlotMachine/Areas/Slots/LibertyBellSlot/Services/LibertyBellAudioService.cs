using Microsoft.Extensions.Logging;
using ModestySlotMachine.Areas.Slots.LibertyBellSlot.Resources;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace ModestySlotMachine.Areas.Slots.LibertyBellSlot.Services
{
    public class LibertyBellAudioService
    {
        private readonly ILogger<LibertyBellAudioService> _logger;
        private readonly IAudioManager _audioManager;
        private IAudioPlayer _currentAudioPlayer;

        public LibertyBellAudioService(ILogger<LibertyBellAudioService> logger, IAudioManager audioManager)
        {
            _logger = logger;
            _audioManager = audioManager;
        }

        public double MusicVolume { get; set; } = 0.5;
        public double FxSoundVolume { get; set; } = 0.5;

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

        public void PlayBackgroundMusic()
        {
            _currentAudioPlayer = _audioManager.CreatePlayer(new MemoryStream(BackgroundTracks.in_the_saloon_116225));
            _currentAudioPlayer.Volume = MusicVolume;
            _currentAudioPlayer.Play();
        }

        public void StopBackgroundMusic()
        {
            if (_currentAudioPlayer == null)
                return;

            _currentAudioPlayer.Stop();
        }

        protected void Play(Stream audioResourceStream)
        {
            var player = _audioManager.CreatePlayer(audioResourceStream);
            player.Volume = FxSoundVolume;
            player.Play();
        }
    }
}
