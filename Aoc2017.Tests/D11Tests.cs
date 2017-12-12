using NUnit.Framework;

namespace Aoc2017.Tests
{
    public class D11Tests
    {
        [Test(Description = "11.1")]
        [TestCase("ne,ne,ne", 3)]
        [TestCase("ne,ne,sw,sw", 0)]
        [TestCase("ne,ne,s,s", 2)]
        [TestCase("se,sw,se,sw,sw", 3)]
        public void TestEstimateP1(string input, int expectedAnswer)
        {
            Assert.AreEqual(expectedAnswer, new D11.D11Solver().EstimateP1(input));
        }
    }
}
