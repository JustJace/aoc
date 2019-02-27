using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Aoc2018.Solutions
{
    public class D16P1 : Solver<int>
    {
        public override int Day => 16;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d16.input";

        protected override int GetAnswer(string input)
        {
            var samples = ParseInput(input);
            var answer = 0;
           
            foreach (var sample in samples)
            {
                var sampleMatch = 0;
                foreach (var testOpCode in Enum.GetValues(typeof(OpCodes)).Cast<OpCodes>().ToList())
                {
                    if (TestOpCode(sample, testOpCode))
                        sampleMatch++;
                }

                if (sampleMatch >= 3)
                    answer++;
            }

            return answer;
        }

        private bool TestOpCode(Sample sample, OpCodes testOpCode)
        {
            Registers result;

            switch (testOpCode)
            {
                case OpCodes.addi: result = addi(sample.Before, sample.Instruction); break;
                case OpCodes.addr: result = addr(sample.Before, sample.Instruction); break;
                case OpCodes.bani: result = bani(sample.Before, sample.Instruction); break;
                case OpCodes.banr: result = banr(sample.Before, sample.Instruction); break;
                case OpCodes.bori: result = bori(sample.Before, sample.Instruction); break;
                case OpCodes.borr: result = borr(sample.Before, sample.Instruction); break;
                case OpCodes.eqir: result = eqir(sample.Before, sample.Instruction); break;
                case OpCodes.eqri: result = eqri(sample.Before, sample.Instruction); break;
                case OpCodes.eqrr: result = eqrr(sample.Before, sample.Instruction); break;
                case OpCodes.gtir: result = gtir(sample.Before, sample.Instruction); break;
                case OpCodes.gtri: result = gtri(sample.Before, sample.Instruction); break;
                case OpCodes.gtrr: result = gtrr(sample.Before, sample.Instruction); break;
                case OpCodes.muli: result = muli(sample.Before, sample.Instruction); break;
                case OpCodes.mulr: result = mulr(sample.Before, sample.Instruction); break;
                case OpCodes.seti: result = seti(sample.Before, sample.Instruction); break;
                case OpCodes.setr: result = setr(sample.Before, sample.Instruction); break;
                default: throw new Exception("Tried testing op code that is not handled.");
            }

            return Registers.Equal(sample.After, result);
        }

        private Registers addr(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = registers[instruction.A] + registers[instruction.B];
            return result;
        }

        private Registers addi(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = registers[instruction.A] + instruction.B;
            return result;
        }

        private Registers mulr(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = registers[instruction.A] * registers[instruction.B];
            return result;
        }

        private Registers muli(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = registers[instruction.A] * instruction.B;
            return result;
        }

        private Registers banr(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = registers[instruction.A] & registers[instruction.B];
            return result;
        }

        private Registers bani(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = registers[instruction.A] & instruction.B;
            return result;
        }

        private Registers borr(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = registers[instruction.A] | registers[instruction.B];
            return result;
        }

        private Registers bori(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = registers[instruction.A] | instruction.B;
            return result;
        }

        private Registers setr(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = registers[instruction.A];
            return result;
        }

        private Registers seti(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = instruction.A;
            return result;
        }

        private Registers gtir(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = instruction.A > registers[instruction.B] ? 1 : 0;
            return result;
        }

        private Registers gtri(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = registers[instruction.A] > instruction.B ? 1 : 0;
            return result;
        }

        private Registers gtrr(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = registers[instruction.A] > registers[instruction.B] ? 1 : 0;
            return result;
        }

        private Registers eqir(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = instruction.A == registers[instruction.B] ? 1 : 0;
            return result;
        }

        private Registers eqri(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = registers[instruction.A] == instruction.B ? 1 : 0;
            return result;
        }

        private Registers eqrr(Registers registers, Instruction instruction)
        {
            var result = registers.Clone();
            result[instruction.C] = registers[instruction.A] == registers[instruction.B] ? 1 : 0;
            return result;
        }

        private List<Sample> ParseInput(string input)
        {
            var lines = input.Split(Environment.NewLine);
            var samples = new List<Sample>();

            for (var i = 0; i < lines.Count(); i += 4)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    break;

                samples.Add(new Sample()
                {
                    Before = ParseRegisters(lines[i]),
                    Instruction = ParseInstruction(lines[i + 1]),
                    After = ParseRegisters(lines[i + 2])
                });
            }

            return samples;
        }

        private static readonly Regex _registerRegex = new Regex(@".+:\s+\[(\d+), (\d+), (\d+), (\d+)\]");
        private Registers ParseRegisters(string line)
        {
            var registers = new Registers();
            var match = _registerRegex.Match(line);
            registers.R0 = int.Parse(match.Groups[1].ToString());
            registers.R1 = int.Parse(match.Groups[2].ToString());
            registers.R2 = int.Parse(match.Groups[3].ToString());
            registers.R3 = int.Parse(match.Groups[4].ToString());
            return registers;
        }

        private static readonly Regex _instructionRegex = new Regex(@"(\d+) (\d+) (\d+) (\d+)");
        private Instruction ParseInstruction(string line)
        {
            var instruction = new Instruction();
            var match = _instructionRegex.Match(line);
            instruction.OpCode = int.Parse(match.Groups[1].ToString());
            instruction.A = int.Parse(match.Groups[2].ToString());
            instruction.B = int.Parse(match.Groups[3].ToString());
            instruction.C = int.Parse(match.Groups[4].ToString());
            return instruction;
        }

        private class Instruction
        {
            public int OpCode { get; set; }
            public int A { get; set; }
            public int B { get; set; }
            public int C { get; set; }
        }

        private class Registers
        {
            public Registers Clone() => MemberwiseClone() as Registers;
            public static bool Equal(Registers r1, Registers r2)
            {
                return r1[0] == r2[0]
                    && r1[1] == r2[1]
                    && r1[2] == r2[2]
                    && r1[3] == r2[3];
            }
            public int R0 { get; set; }
            public int R1 { get; set; }
            public int R2 { get; set; }
            public int R3 { get; set; }
            public int this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0: return R0; 
                        case 1: return R1; 
                        case 2: return R2; 
                        case 3: return R3;
                        default: throw new Exception("Trying to access register that doesn't exist.");
                    }
                }
                set
                {
                    switch (index)
                    {
                        case 0: R0 = value; break;
                        case 1: R1 = value; break;
                        case 2: R2 = value; break;
                        case 3: R3 = value; break;
                        default: throw new Exception("Trying to access register that doesn't exist.");
                    }
                }
            }
        }

        private class Sample
        {
            public Registers Before { get; set; }
            public Registers After { get; set; }
            public Instruction Instruction { get; set; }
        }

        private enum OpCodes
        {
            addr,
            addi,
            mulr,
            muli,
            banr,
            bani,
            borr,
            bori,
            setr,
            seti,
            gtri,
            gtir,
            gtrr,
            eqir,
            eqri,
            eqrr
        }
    }
}
