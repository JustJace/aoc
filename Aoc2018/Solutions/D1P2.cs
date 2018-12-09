using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2018.Solutions
{
    public class D1P2 : Solver<int>
    {
        public override int Day => 1;
        public override int Part => 2;
        protected override string Filename => @"Inputs\d1p1.input";

        protected override int GetAnswer(string input)
        {
            var frequenciesChanges = input.Split(Environment.NewLine).Select(int.Parse);
            var hash = new HashSet<int>();
            var sum = 0;

            while (true)
            {
                foreach (var change in frequenciesChanges)
                {
                    sum += change;
                    if (hash.Contains(sum))
                        return sum;
                    hash.Add(sum);
                }
            }
        }
    }
}
