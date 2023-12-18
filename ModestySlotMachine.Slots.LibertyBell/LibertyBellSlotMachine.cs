
using System;
using System.Collections.ObjectModel;

namespace ModestySlotMachine.Slots.LibertyBell
{
    public class LibertyBellSlotMachine
    {
        const uint ReelAmount = 3;

        #region Configuration properties
        readonly Configuration _configuration;
        readonly Payout<LibertyBellSymbols>[] _payouts;
        #endregion

        #region Internal state properties
        readonly Random _randomGenerator = new();
        readonly LibertyBellSymbols[] _allSymbols = Enum.GetValues<LibertyBellSymbols>();
        public LibertyBellSymbols[] Symbols { get; protected set; } = default!;
        #endregion

        public LibertyBellSlotMachine(Configuration configuration)
        {
            _configuration = configuration;
            _payouts = configuration.Payouts;
        }

        public LibertyBellSymbols[] MakeSpin()
        {
            Symbols = new LibertyBellSymbols[ReelAmount];

            for (uint reelIndex = 0; reelIndex < ReelAmount; reelIndex++)
            {
                Symbols[reelIndex] = GetRandomSymbol();
            }

            return Symbols;
        }

        public LibertyBellSymbols GetRandomSymbol()
        {
            int number = _randomGenerator.Next(0, _allSymbols.Length);
            return _allSymbols[number];
        }

        public bool HasPayout(out Payout<LibertyBellSymbols> result)
        {
            result = default!;

            if (Symbols == null)
                return false;

            foreach (var payout in _payouts.OrderByDescending(payout => payout.Reward))
            {
                bool isMet = new PayoutRequirementsValidator<LibertyBellSymbols>().IsRequirementMet(payout.Requirements, Symbols);
                if (isMet)
                {
                    result = payout;
                    return true;
                }
            }
            return false;
        }

        public class ReelStopArgs : EventArgs
        {
            public uint ReelIndex { get; set; } 
            public LibertyBellSymbols Symbol { get; set; }
        }

    }
}
