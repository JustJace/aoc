using System;
using System.Collections.Generic;
using System.Text;

namespace Aoc2018.Solutions
{
    public class D05P1 : Solver<int>
    {
        public override int Day => 5;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d05.input";

        protected override int GetAnswer(string input)
        {
            var i = 0;
            var polymer = input;
            while (i + 1 < polymer.Length)
            {
                var c1 = polymer[i];
                var c2 = polymer[i+1];

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
