using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aoc2017.Tests
{
    public class D3Tests
    {
        [Test(Description = "3.1")]
        [TestCase(1, 0)]
        [TestCase(12, 3)]
        [TestCase(23, 2)]
        [TestCase(25, 4)]
        [TestCase(1024, 31)]
        public void Test_EstimateP1(int input, int expectedAnswer)
        {
            Assert.AreEqual(expectedAnswer, new D3.D3Solver().EstimateP1(input));
        }

        [Test(Description = "3.2")]
        [TestCase(1, 2)]
        [TestCase(2, 4)]
        [TestCase(5, 10)]
        [TestCase(57, 59)]
        [TestCase(59, 122)]
        [TestCase(142, 147)]
        [TestCase(147, 304)]
        public void Test_BruteForceP2(int input, int expectedAnswer)
        {
            Assert.AreEqual(expectedAnswer, new D3.D3Solver().BruteForceP2(input));
        }
    }
}
