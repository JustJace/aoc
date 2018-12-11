using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2018.Solutions
{
    public class D05P2 : Solver<int>
    {
        public override int Day => 5;
        public override int Part => 2;
        protected override string Filename => @"Inputs\d05.input";

        protected override int GetAnswer(string input)
        {
            var best = int.MaxValue;

            for (var c = 65; c < 91; c++)
            {
                var testPolymer = new List<char>(input);
                testPolymer.RemoveAll(p => p == (char)c);
                testPolymer.RemoveAll(p => p == (char)(c + 32));

                var reactedLength = FullyReactedPolymerLength(testPolymer);

                if (reactedLength < best)
                    best = reactedLength;
            }

            return best;
        }

        private int FullyReactedPolymerLength(List<char> polymer)
        {
            var i = 0;
            while (i + 1 < polymer.Count)
            {
                var c1 = polymer[i];
                var c2 = polymer[i + 1];

                if (AreReversedPolarity(c1, c2))
                {
                    polymer.RemoveAt(i);
                    polymer.RemoveAt(i);
                    i--;
                    if (i < 0) i = 0;
                }
                else
                {
                    i++;
                }
            }

            return polymer.Count;
        }

        private bool AreReversedPolarity(char c1, char c2) => 32 == Math.Abs(c1 - c2);
    }
}
