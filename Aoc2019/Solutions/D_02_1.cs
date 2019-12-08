using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Aoc2019.Solutions
{
    public class D_02_1 : Solver<int>
    {
        public override int Day => 2;

        public override int Part => 1;

        protected override string Filename => @"Inputs\D_02.input";

        protected override int GetAnswer(string input)
        {
            var memory = input.Split(',').Select(int.Parse).ToArray();

            memory[1] = 12;
            memory[2] = 2;

            var pointer = 0;

            while (pointer >= 0 && pointer < memory.Length && memory[pointer] != 99)
            {
                var opCode = memory[pointer];
                var val1 = memory[memory[pointer + 1]];
                var val2 = memory[memory[pointer + 2]];
                var storagePoint = memory[pointer + 3];

                switch (opCode)
                {
                    case 1: memory[storagePoint] = val1 + val2; break;
                    case 2: memory[storagePoint] = val1 * val2; break;
                    default: return memory[0];
                }

                pointer += 4;
            }

            return memory[0];
        }
    }
}
