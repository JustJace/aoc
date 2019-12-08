using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Aoc2019.Solutions
{
    public class D_04_1 : Solver<int>
    {
        public override int Day => 4;

        public override int Part => 1;

        protected override string Filename => @"Inputs\D_04.input";

        protected override int GetAnswer(string input)
        {
            var min = 240298;
            var max = 784956;

            var count = 0;

            for (var i = min; i <= max; i++)
            {
                if (!HasDouble(i))
                    continue;

                if (!Increases(i))
                    continue;

                count++;
            }

            return count;
        }

        private bool Increases(int number)
        {
            var s = number.ToString();

            for (var i = 0; i < s.Length - 1; i++)
            {
                if (int.Parse(s[i].ToString()) > int.Parse(s[i + 1].ToString()))
                    return false;
            }

            return true;
        }

        private bool HasDouble(int i)
        {
            return i.ToString().ToCharArray().GroupBy(c => c).Any(g => g.Count() > 1);
        }
    }
}
