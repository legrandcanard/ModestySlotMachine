using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModestSlotMachine.Core.Audio
{
    public class Track
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Stream AudioStream { get; set; }
    }
}
