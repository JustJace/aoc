using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2018.Solutions
{
    public class D2P1 : Solver<int>
    {
        public override int Day => 2;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d2p1.input";

        protected override int GetAnswer(string input)
        {
            var ids = input.Split(Environment.NewLine);
            var twos = 0;
            var threes = 0;

            foreach (var id in ids)
            {
                var groups = id.GroupBy(c => c);
                if (groups.Any(g => g.Count() == 2))
                    twos++;

                if (groups.Any(g => g.Count() == 3))
                    threes++;
            }

            return twos * threes;
        }
    }
}
