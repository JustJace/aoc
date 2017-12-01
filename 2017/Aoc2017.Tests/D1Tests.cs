using NUnit.Framework;
using System;

namespace Aoc2017.Tests
{
    public class D1Tests
    {
        [TestCase("1122", 3)]
        [TestCase("1111", 4)]
        [TestCase("1234", 0)]
        [TestCase("91212129", 9)]
        [Test(Description = "1.1")]
        public void Test_BruteForceP1(string input, int answer)
        {
            Assert.AreEqual(answer, D1.D1Solver.BruteForceP1(input));
        }

        [TestCase("1212", 6)]
        [TestCase("1221", 0)]
        [TestCase("123425", 4)]
        [TestCase("123123", 12)]
        [TestCase("12131415", 4)]
        [Test(Description = "1.2")]
        public void Test_BruteForceP2(string input, int answer)
        {
            Assert.AreEqual(answer, D1.D1Solver.BruteForceP2(input));
        }
    }
}
