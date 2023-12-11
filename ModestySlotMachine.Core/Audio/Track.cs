using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModestySlotMachine.Core.Audio
{
    public class Track
    {
        public required string Name { get; set; }
        public required Stream AudioStream { get; set; }
    }
}
