using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModestySlotMachine.Core.Audio
{
    public class TrackEventArgs : EventArgs
    {
        public Track Track { get; set; } = null!;
        public double TrackTime { get; set; }
        public int Index { get; set; }
    }
}
