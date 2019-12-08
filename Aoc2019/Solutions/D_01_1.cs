using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Aoc2019.Solutions
{
    public class D_01_1 : Solver<int>
    {
        public override int Day => 1;

        public override int Part => 1;

        protected override string Filename => @"Inputs\D_01.input";

        protected override int GetAnswer(string input)
        {
            return input.Split(Environment.NewLine)
                        .Select(int.Parse)
                        .Select(CalculateMassBasedFuelReq)
                        .Sum();
        }

        private int CalculateMassBasedFuelReq(int mass) => mass / 3 - 2;
    }
}
