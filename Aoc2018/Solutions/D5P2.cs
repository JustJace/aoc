using System;
using System.Collections.Generic;
using System.Text;

namespace Aoc2018.Solutions
{
    public class D5P2 : Solver<int>
    {
        public override int Day => 5;
        public override int Part => 2;
        protected override string Filename => @"Inputs\d5p1.input";

        protected override int GetAnswer(string input)
        {
            var best = int.MaxValue;

            for (var c = 65; c < 91; c++)
            {
                var testPolymer = input.Replace(((char)c).ToString(), "")
                                       .Replace(((char)(c + 32)).ToString(), "");

                var reactedLength = FullyReactedPolymerLength(testPolymer);

                if (reactedLength < best)
                    best = reactedLength;
            }

            return best;
        }

        private int FullyReactedPolymerLength(string polymer)
        {
            var i = 0;
            while (i + 1 < polymer.Length)
            {
                var c1 = polymer[i];
                var c2 = polymer[i + 1];

                if (AreReversedPolarity(c1, c2))
                {
                    polymer = polymer.Remove(i, 2);
                    i--;
                    if (i < 0) i = 0;
                }
                else
                {
                    i++;
                }
            }

            return polymer.Length;
        }

        private bool AreReversedPolarity(char c1, char c2) => 32 == Math.Abs(c1 - c2);
    }
}
