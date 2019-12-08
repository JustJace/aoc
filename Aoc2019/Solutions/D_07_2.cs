using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2019.Solutions
{
    public class D_07_2 : Solver<int>
    {
        public override int Day => 7;

        public override int Part => 2;

        protected override string Filename => @"Inputs\D_07.input";

        protected override int GetAnswer(string input)
        {
            //input = @"3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5";
            //input = @"3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10";
            var intCodeProgram = input.Split(',').Select(int.Parse).ToArray();
            var phaseSettings = new int[] { 5, 6, 7, 8, 9 };
            var phaseSettingPermutations = GetPermutations<int>(phaseSettings, 5).ToList();

            var best = 0;

            foreach (var permutation in phaseSettingPermutations)
            {
                var thrustInput = RunProgramOnAmplifiers(intCodeProgram, permutation.ToArray());
                if (thrustInput > best)
                    best = thrustInput;
            }

            return best;
        }

        // stole this from stackoverflow
        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        private int RunProgramOnAmplifiers(int[] intCodeProgram, int[] phaseSettings)
        {
            var lastOutput = 0;
            var computers = phaseSettings
                .Select(ps => new IntcodeComputer(intCodeProgram.Clone() as int[], new int[] { ps }))
                .ToArray();

            while (computers.Any(c => !c.Halted))
            {
                for (var i = 0; i < 5; i++)
                {
                    var computer = computers[i];
                    computer.InputQueue.Enqueue(lastOutput);
                    computer.Run();
                    lastOutput = computer.OutputBuffer.Last();
                    computer.OutputBuffer.Clear();
                }
            }

          
            return lastOutput;
        }

        private class IntcodeComputer
        {
            public int[] Memory { get; }
            public Queue<int> InputQueue { get; } = new Queue<int>();
            public List<int> OutputBuffer { get; }
            public bool Halted { get; private set; }
            private int _iPointer = 0;

            public IntcodeComputer(int[] memory, IEnumerable<int> inputs)
            {
                Memory = memory;
                foreach (var input in inputs)
                {
                    InputQueue.Enqueue(input);
                }
                OutputBuffer = new List<int>();
            }

            public void Run()
            {
                while (_iPointer >= 0 && _iPointer < Memory.Length)
                {
                    var instruction = Memory[_iPointer];
                    var opCode = instruction % 100;

                    switch (opCode)
                    {
                        case 1: Add(); break;
                        case 2: Multiply(); break;
                        case 3: if (!InputQueue.Any()) return; ReadInput(); break;
                        case 4: WriteOutput(); break;
                        case 5: JumpIfTrue(); break;
                        case 6: JumpIfFalse(); break;
                        case 7: LessThan(); break;
                        case 8: Equals(); break;
                        case 99: Halted = true; return;
                        default: throw new Exception($"Unexpected opcode: {opCode}");
                    }
                }
            }

            private int GetParameter(int paramIndex)
            {
                var parameterMode = (Memory[_iPointer] / (int)(Math.Pow(10, paramIndex + 1))) % 10;

                switch (parameterMode)
                {
                    case 0: return Memory[Memory[_iPointer + paramIndex]];
                    case 1: return Memory[_iPointer + paramIndex];
                    default: throw new Exception($"Unexpected Parameter Mode: {parameterMode}");
                }
            }

            private void Add()
            {
                var param1 = GetParameter(1);
                var param2 = GetParameter(2);
                Memory[Memory[_iPointer + 3]] = param1 + param2;
                _iPointer += 4;
            }

            private void Multiply()
            {
                var param1 = GetParameter(1);
                var param2 = GetParameter(2);
                Memory[Memory[_iPointer + 3]] = param1 * param2;
                _iPointer += 4;
            }

            private void ReadInput()
            {
                Memory[Memory[_iPointer + 3]] = InputQueue.Dequeue();
                _iPointer += 2;
            }

            private void WriteOutput()
            {
                var param1 = GetParameter(1);
                OutputBuffer.Add(param1);
                _iPointer += 2;
            }

            private void JumpIfTrue()
            {
                var param1 = GetParameter(1);
                var param2 = GetParameter(2);
                if (param1 != 0)
                {
                    _iPointer = param2;
                }
                else
                {
                    _iPointer += 3;
                }
            }

            private void JumpIfFalse()
            {
                var param1 = GetParameter(1);
                var param2 = GetParameter(2);
                if (param1 == 0)
                {
                    _iPointer = param2;
                }
                else
                {
                    _iPointer += 3;
                }
            }

            private void LessThan()
            {
                var param1 = GetParameter(1);
                var param2 = GetParameter(2);
                Memory[Memory[_iPointer + 3]] = param1 < param2 ? 1 : 0;
                _iPointer += 4;
            }

            private void Equals()
            {
                var param1 = GetParameter(1);
                var param2 = GetParameter(2);
                Memory[Memory[_iPointer + 3]] = param1 == param2 ? 1 : 0;
                _iPointer += 4;
            }
        }
    }
}
