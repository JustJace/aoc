using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Aoc2017.D4
{
    public class D4Solver
    {
        public int SolveP1() => BruteForceP1(File.ReadAllText(@"D4\D4.input"));

        internal int BruteForceP1(string input)
        {
            var validCount = 0;
            var passphrases = input.Split(Environment.NewLine);

            foreach (var passphrase in passphrases)
                if (IsValidPassphraseP1(passphrase))
                    validCount++;

            return validCount;
        }

        internal bool IsValidPassphraseP1(string passphrase)
        {
            var hashset = new HashSet<string>();
            var words = passphrase.Split(' ');
            foreach(var word in words)
            {
                if (hashset.Contains(word)) return false;

                hashset.Add(word);
            }

            return true;
        }

        public int SolveP2() => BruteForceP2(File.ReadAllText(@"D4\D4.input"));

        internal int BruteForceP2(string input)
        {
            var validCount = 0;
            var passphrases = input.Split(Environment.NewLine);

            foreach (var passphrase in passphrases)
                if (IsValidPassphraseP2(passphrase))
                    validCount++;

            return validCount;
        }

        internal bool IsValidPassphraseP2(string passphrase)
        {
            var hashset = new HashSet<string>();
            var words = passphrase.Split(' ')
                                  .Select(w => new string(w.OrderBy(c => c)
                                                           .ToArray()));
            foreach (var word in words)
            {
                if (hashset.Contains(word)) return false;

                hashset.Add(word);
            }

            return true;
        }
    }
}
