using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aoc2017.Tests
{
    public class D2Tests
    {
        [Test(Description = "2.1")]
        [TestCase(@"5 1 9 5
7 5 3
2 4 6 8", 18)]
        public void Test_BruteForceP1(string input, int expectedAnswer)
        {
            Assert.AreEqual(expectedAnswer, new D2.D2Solver().BruteForceP1(input));
        }

        [Test(Description = "2.2")]
        [TestCase(@"5 9 2 8
9 4 7 3
3 8 6 5", 9)]
        public void Test_BruteForceP2(string input, int expectedAnswer)
        {
            Assert.AreEqual(expectedAnswer, new D2.D2Solver().BruteForceP2(input));
        }
    }
}
