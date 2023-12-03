namespace ModestySlotMachine.Slots.LibertyBell.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            var slot = new LibertyBellSlotMachine(new Configuration
            {
                Payouts = new[]
                {
                    new Payout<LibertyBellSymbols>(new[] { new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Horseshoes, 2) }, 5),
                    new Payout<LibertyBellSymbols>(
                        new[] {
                            new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Horseshoes, 2),
                            new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Star, 1)
                        }, 10)
                }
            });

            await slot.PullAsync();
            
            if (slot.HasPayout(out Payout<LibertyBellSymbols> payout))
            {

            }


            Assert.Pass();
        }
    }
}