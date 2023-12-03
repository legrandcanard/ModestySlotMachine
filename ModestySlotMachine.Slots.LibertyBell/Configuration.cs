using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModestySlotMachine.Slots.LibertyBell
{
    public class Configuration
    {
        public Payout<LibertyBellSymbols>[] Payouts { get; set; } = default!;
    }

    public record Requirement<TSymbol>(TSymbol Symbol, int Amount);

    public record Payout<TSymbol>(Requirement<TSymbol>[] Requirements, decimal Reward, string Title);
}
