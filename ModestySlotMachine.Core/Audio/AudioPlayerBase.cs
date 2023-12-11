using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ModestySlotMachine.Core.Audio
{
    public abstract class AudioPlayerBase
    {
        protected IAudioManager AudioManager { get; private set; }
        public double Volume { get; set; } = 0.5;

        public AudioPlayerBase(IAudioManager audioManager)
        {
            AudioManager = audioManager;
        }

        protected IAudioPlayer CreatePlayer(Stream audioResourceStream)
        {
            var player = AudioManager.CreatePlayer(audioResourceStream);
            player.Volume = Volume;
            return player;
        }

        protected IAudioPlayer Play(Stream audioResourceStream, bool loop = false)
        {
            var player = CreatePlayer(audioResourceStream);
            player.Loop = loop;
            player.Play();
            return player;
        }
    }
}
