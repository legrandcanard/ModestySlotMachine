
namespace ModestySlotMachine.Slots.LibertyBell
{
    public class PayoutRequirementsValidator<TSymbol>
        where TSymbol : struct, Enum
    {
        public bool IsRequirementMet(Requirement<TSymbol>[] requirements, TSymbol[] libertyBellSymbols)
        {
            Dictionary<TSymbol, int> requirementsTable = BuildRequirementsTable(requirements);

            foreach (var actualSymbol in libertyBellSymbols)
            {
                --requirementsTable[actualSymbol];
            }

            bool isMet = requirementsTable.All(pair => pair.Value <= 0);
            return isMet;
        }

        protected Dictionary<TSymbol, int> BuildRequirementsTable(Requirement<TSymbol>[] requirements)
        {
            Dictionary<TSymbol, int> requirementsTable = Enum.GetValues<TSymbol>().ToDictionary(value => value, _ => 0);

            foreach (var requirement in requirements)
            {
                requirementsTable[requirement.Symbol] = requirement.Amount;
            }

            return requirementsTable;
        }
    }
}
