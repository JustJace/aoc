using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aoc2017.Tests
{
    public class D5Tests
    {
        [Test(Description = "5.1")]
        [TestCase(@"0
3
0
1
-3", 5)]
        public void Test_BruteForceP1(string input, int expectedAnswer)
        {
            Assert.AreEqual(expectedAnswer, new D5.D5Solver().BruteForceP1(input));
        }

        [Test(Description = "5.2")]
        [TestCase(@"0
3
0
1
-3", 10)]
        public void Test_BruteForceP2(string input, int expectedAnswer)
        {
            Assert.AreEqual(expectedAnswer, new D5.D5Solver().BruteForceP2(input));
        }
    }
}
