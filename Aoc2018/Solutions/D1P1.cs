using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Aoc2018.Solutions
{
    public class D1P1 : Solver<int>
    {
        public override int Day => 1;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d1p1.input";

        protected override int GetAnswer(string input)
        {
            return input.Split(Environment.NewLine).Select(int.Parse).Sum();
        }
    }
}
