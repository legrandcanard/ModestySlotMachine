using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModestSlotMachine.Slots.LibertyBell.Tests
{
    public class PayoutRequirementsValidator_Tests
    {
        [TestCase(LibertyBellSymbols.Horseshoes, LibertyBellSymbols.Horseshoes, LibertyBellSymbols.Horseshoes, ExpectedResult = true)]
        [TestCase(LibertyBellSymbols.Horseshoes, LibertyBellSymbols.Horseshoes, LibertyBellSymbols.Spades, ExpectedResult = false)]
        [TestCase(LibertyBellSymbols.Star, LibertyBellSymbols.Diamonds, LibertyBellSymbols.Spades, ExpectedResult = false)]
        public bool IsRequirementMet_Single_Requirement_Test(params LibertyBellSymbols[] symbols)
        {
            var result = new PayoutRequirementsValidator<LibertyBellSymbols>().IsRequirementMet(
                new[]
                {
                    new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Horseshoes, 3)
                },
                symbols
            );
            return result;
        }

        [TestCase(LibertyBellSymbols.Horseshoes, LibertyBellSymbols.Horseshoes, LibertyBellSymbols.Horseshoes, ExpectedResult = false)]
        [TestCase(LibertyBellSymbols.Horseshoes, LibertyBellSymbols.Horseshoes, LibertyBellSymbols.Spades, ExpectedResult = false)]
        [TestCase(LibertyBellSymbols.Star, LibertyBellSymbols.Diamonds, LibertyBellSymbols.Spades, ExpectedResult = false)]
        [TestCase(LibertyBellSymbols.Star, LibertyBellSymbols.Star, LibertyBellSymbols.Spades, ExpectedResult = true)]
        [TestCase(LibertyBellSymbols.Star, LibertyBellSymbols.Spades, LibertyBellSymbols.Star, ExpectedResult = true)]
        [TestCase(LibertyBellSymbols.Spades, LibertyBellSymbols.Star, LibertyBellSymbols.Star, ExpectedResult = true)]
        public bool IsRequirementMet_Multiple_Requirements_Test(params LibertyBellSymbols[] symbols)
        {
            var result = new PayoutRequirementsValidator<LibertyBellSymbols>().IsRequirementMet(
                new[]
                {
                    new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Star, 2),
                    new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Spades, 1)
                },
                symbols
            );
            return result;
        }

        [TestCase(LibertyBellSymbols.Horseshoes, LibertyBellSymbols.Horseshoes, LibertyBellSymbols.Horseshoes, ExpectedResult = false)]
        [TestCase(LibertyBellSymbols.Horseshoes, LibertyBellSymbols.Horseshoes, LibertyBellSymbols.Spades, ExpectedResult = false)]
        [TestCase(LibertyBellSymbols.Star, LibertyBellSymbols.Diamonds, LibertyBellSymbols.Spades, ExpectedResult = true)]
        [TestCase(LibertyBellSymbols.Star, LibertyBellSymbols.Star, LibertyBellSymbols.Spades, ExpectedResult = false)]
        [TestCase(LibertyBellSymbols.Star, LibertyBellSymbols.Spades, LibertyBellSymbols.Star, ExpectedResult = false)]
        [TestCase(LibertyBellSymbols.Spades, LibertyBellSymbols.Star, LibertyBellSymbols.Star, ExpectedResult = false)]
        [TestCase(LibertyBellSymbols.Star, LibertyBellSymbols.Spades, LibertyBellSymbols.Diamonds, ExpectedResult = true)]
        [TestCase(LibertyBellSymbols.Diamonds, LibertyBellSymbols.Star, LibertyBellSymbols.Spades, ExpectedResult = true)]
        public bool IsRequirementMet_Requirement_Per_Each_Reel_Test(params LibertyBellSymbols[] symbols)
        {
            var result = new PayoutRequirementsValidator<LibertyBellSymbols>().IsRequirementMet(
                new[]
                {
                    new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Star, 1),
                    new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Spades, 1),
                    new Requirement<LibertyBellSymbols>(LibertyBellSymbols.Diamonds, 1)
                },
                symbols
            );
            return result;
        }
    }
}
