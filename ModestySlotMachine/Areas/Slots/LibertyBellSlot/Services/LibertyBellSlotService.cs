using ModestySlotMachine.Slots.LibertyBell;
using ModestySlotMachine.Areas.Slots.LibertyBellSlot.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModestySlotMachine.Areas.Slots.LibertyBellSlot.Services
{
    public class LibertyBellSlotService
    {
        public LibertyBellSlotService() { }

        public LibertyBellSlotMachine CreateSlotMachine()
        {
            var slot = new LibertyBellSlotMachine(new Configuration
            {
                Payouts = GetPayouts()
            });

            return slot;
        }

        public Payout<LibertyBellSymbols>[] GetPayouts()
        {
            return new[]
            {
                new Payout<LibertyBellSymbols>(
                    new[] { new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Horseshoes, 2) }, 5, Localization.Payout_2Horseshoes),
                new Payout<LibertyBellSymbols>(
                    new[] {
                        new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Horseshoes, 2),
                        new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Star, 1)
                    }, 10, Localization.Payout_2Horseshoes1Star),
                new Payout<LibertyBellSymbols>(
                    new[] { new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Spades, 3) }, 20, Localization.Payout_3Spades),
                new Payout<LibertyBellSymbols>(
                    new[] { new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Diamonds, 3) }, 30, Localization.Payout_3Diamonds),
                new Payout<LibertyBellSymbols>(
                    new[] { new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Hearts, 3) }, 40, Localization.Payout_3Hearts),
                new Payout<LibertyBellSymbols>(
                    new[] { new Requirement<LibertyBellSymbols>(LibertyBellSymbols.LibertyBell, 3) }, 50, Localization.Payout_3Bells)
            };
        }
    }
}
