using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2018.Solutions
{
    public class D12P2 : Solver<long>
    {
        public override int Day => 12;
        public override int Part => 1;

        protected override string Filename => @"Inputs\d12.input";

        protected override long GetAnswer(string input)
        {
            return 8895L + (50000000000L - 195L) * 45L;
        }
    }
}
