using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aoc2017.Tests
{
    public class D4Tests
    {
        [Test(Description = "4.1")]
        [TestCase("aa bb cc dd ee", true)]
        [TestCase("aa bb cc dd aa", false)]
        [TestCase("aa bb cc dd aaa", true)]
        public void Test_IsValidPassphraseP1(string passphrase, bool expectedValue)
        {
            Assert.AreEqual(expectedValue, new D4.D4Solver().IsValidPassphraseP1(passphrase));
        }

        [Test(Description = "4.2")]
        [TestCase("abcde fghij", true)]
        [TestCase("abcde xyz ecdab", false)]
        [TestCase("a ab abc abd abf abj", true)]
        [TestCase("iiii oiii ooii oooi oooo", true)]
        [TestCase("oiii ioii iioi iiio", false)]
        public void Test_IsValidPassphraseP2(string passphrase, bool expectedValue)
        {
            Assert.AreEqual(expectedValue, new D4.D4Solver().IsValidPassphraseP2(passphrase));
        }
    }
}
