namespace ModestySlotMachine.Slots.LibertyBell.Tests
{
    public class LibertyBellSlotMachine_Tests
    {
        LibertyBellSlotMachine _libertyBellSlotMachine;

        [SetUp]
        public void Setup()
        {
            _libertyBellSlotMachine = new LibertyBellSlotMachine(new Configuration
            {
                Payouts = new[]
                {
                    new Payout<LibertyBellSymbols>(new[] { new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Horseshoes, 2) }, 5, "Combination 1"),
                    new Payout<LibertyBellSymbols>(
                        new[] {
                            new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Horseshoes, 2),
                            new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Star, 1)
                        }, 10, "Combination 2")
                }
            });
        }

        [Test]
        public void MakeSpin_Returns_3_Symbols()
        {
            var symbols = _libertyBellSlotMachine.MakeSpin();
            Assert.IsTrue(symbols.Length == 3);
        }
    }
}