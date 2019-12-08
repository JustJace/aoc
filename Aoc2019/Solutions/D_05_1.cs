using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Aoc2019.Solutions
{
    public class D_05_1 : Solver<int>
    {
        public override int Day => 5;

        public override int Part => 1;

        protected override string Filename => @"Inputs\D_05.input";

        protected override int GetAnswer(string input)
        {
            var memory = input.Split(',').Select(int.Parse).ToArray();

            var programInput = new Queue<int>();
            programInput.Enqueue(1);

            var cpu = new IntcodeComputer(memory, programInput);
            cpu.Run();
            return cpu.OutputBuffer.Last();
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
                Memory[Memory[_iPointer + 1]] = InputQueue.Dequeue();
                _iPointer += 2;
            }

            private void WriteOutput()
            {
                var param1 = GetParameter(1);
                OutputBuffer.Add(param1);
                _iPointer += 2;
            }
        }
    }
}
