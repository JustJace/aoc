using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2019.Solutions
{
    public class D_07_1 : Solver<int>
    {
        public override int Day => 7;

        public override int Part => 1;

        protected override string Filename => @"Inputs\D_07.input";

        protected override int GetAnswer(string input)
        {
            var intCodeProgram = input.Split(',').Select(int.Parse).ToArray();
            var phaseSettings = new int[] { 0, 1, 2, 3, 4 };
            var phaseSettingPermutations = GetPermutations<int>(phaseSettings, 5);

            var best = 0;

            foreach (var permutation in phaseSettingPermutations)
            {
                var thrustInput = RunProgramOnAmplifiers(intCodeProgram, permutation.ToArray());
                if (thrustInput > best)
                    best = thrustInput;
            }

            return best;
        }

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
            for (var i = 0; i < 5; i++)
            {
                var memory = intCodeProgram.Clone() as int[];
                var input = new Queue<int>();
                input.Enqueue(phaseSettings[i]);
                input.Enqueue(lastOutput);
                var computer = new IntcodeComputer(memory, input);
                computer.Run();
                lastOutput = computer.OutputBuffer.Last();
            }
            return lastOutput;
        }

        private class IntcodeComputer
        {
            public int[] Memory { get; }
            public Queue<int> InputQueue { get; }
            public List<int> OutputBuffer { get; }

            private int _iPointer = 0;

            public IntcodeComputer(int[] memory, Queue<int> inputQueue)
            {
                Memory = memory;
                InputQueue = inputQueue;
                OutputBuffer = new List<int>();
            }

            public void Run()
            {
                while (_iPointer >= 0 && _iPointer < Memory.Length && Memory[_iPointer] != 99)
                {
                    var instruction = Memory[_iPointer];
                    var opCode = instruction % 100;

                    switch (opCode)
                    {
                        case 1: Add(); break;
                        case 2: Multiply(); break;
                        case 3: ReadInput(); break;
                        case 4: WriteOutput(); break;
                        case 5: JumpIfTrue(); break;
                        case 6: JumpIfFalse(); break;
                        case 7: LessThan(); break;
                        case 8: Equals(); break;
                        default: throw new Exception($"Unexpected opcode: {opCode}");
                    }
                }
            }

            private int GetParameter(int paramIndex)
            {
                var parameterMode = (Memory[_iPointer] / (int)(Math.Pow(10, paramIndex + 1))) % 10;

                switch (parameterMode)
                {
                    case 1: return Memory[_iPointer + paramIndex];
                    case 0: return Memory[Memory[_iPointer + paramIndex]];
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
