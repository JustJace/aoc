using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Aoc2019.Solutions
{
    public class D_01_2 : Solver<int>
    {
        public override int Day => 1;

        public override int Part => 2;

        protected override string Filename => @"Inputs\D_01.input";

        protected override int GetAnswer(string input)
        {
            return input.Split(Environment.NewLine)
                        .Select(int.Parse)
                        .Select(CalculateMassBasedFuelReq)
                        .Sum();
        }

        private int CalculateMassBasedFuelReq(int mass)
        {
            var fuelReq = mass / 3 - 2;

            if (fuelReq <= 0)
                return 0;

            return fuelReq + CalculateMassBasedFuelReq(fuelReq);
        }
    }
}
