using Microsoft.Extensions.Logging;
using ModestySlotMachine.Areas.Slots.LibertyBellSlot.Resources;
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
        private ILogger<LibertyBellAudioService> _logger;

        public LibertyBellAudioService(ILogger<LibertyBellAudioService> logger)
        {
            _logger = logger;
        }

        public void PlayRegularWinSound()
        {
            Play(Audio.fairy_arcade_sparkle);
        }

        public void PlayBigWinSound()
        {
            Play(Audio.fairy_arcade_sparkle);
        }

        public void PlayConisFallSound()
        {
            Play(Audio.clinking_coins);
        }

        protected void Play(Stream audioResourceStream)
        {
            try
            {
                using (var soundPlayer = new SoundPlayer(audioResourceStream))
                {
                    soundPlayer.Play();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to play audio.", audioResourceStream);
            }
        }
    }
}
