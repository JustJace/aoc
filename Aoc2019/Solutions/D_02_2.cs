using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Aoc2019.Solutions
{
    public class D_02_2 : Solver<int>
    {
        public override int Day => 2;

        public override int Part => 2;

        protected override string Filename => @"Inputs\D_02.input";

        protected override int GetAnswer(string input)
        {
            var vanillaMemory = input.Split(',').Select(int.Parse).ToArray();
            
            for (var noun = 0; noun <= 99; noun++)
            {
                for (var verb = 0; verb <= 99; verb++)
                {
                    var memory = vanillaMemory.Clone() as int[];
                    memory[1] = noun;
                    memory[2] = verb;

                    var output = RunIntCodeProgram(memory);

                    if (output == 19690720)
                        return 100 * noun + verb;
                }
            }

            throw new Exception("Should've gotten an answer");
        }

        private int RunIntCodeProgram(int[] memory)
        {
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
