using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Aoc2018.Solutions
{
    public class D16P2 : Solver<int>
    {
        public override int Day => 16;
        public override int Part => 2;
        protected override string Filename => @"Inputs\d16.input";

        protected override int GetAnswer(string input)
        {
            var (samples, program) = ParseInput(input);
            var behaviourMap = new Dictionary<int, List<OpCode>>();
            for (var i = 0; i < 16; i++)
                behaviourMap[i] = Enum.GetValues(typeof(OpCode)).Cast<OpCode>().ToList();

            foreach (var sample in samples)
            {
                var removeCodes = new List<OpCode>();
                foreach (var testOpCode in behaviourMap[sample.Instruction.OpNum])
                {
                    if (!TestOpCode(sample, testOpCode))
                        removeCodes.Add(testOpCode);
                }

                foreach (var removeCode in removeCodes)
                    behaviourMap[sample.Instruction.OpNum].Remove(removeCode);

                if (behaviourMap[sample.Instruction.OpNum].Count == 1)
                {
                    for (var i = 0; i < 16; i++)
                    {
                        if (i == sample.Instruction.OpNum) continue;
                        behaviourMap[i].Remove(behaviourMap[sample.Instruction.OpNum].Single());
                    }
                }
            }

            var instructionManual = new Dictionary<int, OpCode>();
            for (var i = 0; i < 16; i++)
                instructionManual[i] = behaviourMap[i].Single();

            var registers = new Registers();
            foreach (var instruction in program)
                registers = RunOp(registers, instruction, instructionManual[instruction.OpNum]);

            return registers[0];
        }

        private bool TestOpCode(Sample sample, OpCode testOpCode)
        {
            var result = RunOp(sample.Before, sample.Instruction, testOpCode);
            return Registers.Equal(sample.After, result);
        }

        private Registers RunOp(Registers registers, Instruction instruction, OpCode opCode)
        {
            switch (opCode)
            {
                case OpCode.addi: return addi(registers, instruction);
                case OpCode.addr: return addr(registers, instruction);
                case OpCode.bani: return bani(registers, instruction);
                case OpCode.banr: return banr(registers, instruction);
                case OpCode.bori: return bori(registers, instruction);
                case OpCode.borr: return borr(registers, instruction);
                case OpCode.eqir: return eqir(registers, instruction);
                case OpCode.eqri: return eqri(registers, instruction);
                case OpCode.eqrr: return eqrr(registers, instruction);
                case OpCode.gtir: return gtir(registers, instruction);
                case OpCode.gtri: return gtri(registers, instruction);
                case OpCode.gtrr: return gtrr(registers, instruction);
                case OpCode.muli: return muli(registers, instruction);
                case OpCode.mulr: return mulr(registers, instruction);
                case OpCode.seti: return seti(registers, instruction);
                case OpCode.setr: return setr(registers, instruction);  
                default: throw new Exception("Tried testing op code that is not handled.");
            }
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

        private Tuple<List<Sample>,List<Instruction>> ParseInput(string input)
        {
            var lines = input.Split(Environment.NewLine);
            var samples = new List<Sample>();
            var instructions = new List<Instruction>();

            var i = 0;
            while (true)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    break;

                samples.Add(new Sample()
                {
                    Before = ParseRegisters(lines[i]),
                    Instruction = ParseInstruction(lines[i + 1]),
                    After = ParseRegisters(lines[i + 2])
                });

                i += 4;
            }

            for (;i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                instructions.Add(ParseInstruction(lines[i]));
            }
            
            return new Tuple<List<Sample>, List<Instruction>>(samples, instructions);
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
            instruction.OpNum = int.Parse(match.Groups[1].ToString());
            instruction.A = int.Parse(match.Groups[2].ToString());
            instruction.B = int.Parse(match.Groups[3].ToString());
            instruction.C = int.Parse(match.Groups[4].ToString());
            return instruction;
        }

        private class Instruction
        {
            public int OpNum { get; set; }
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

        private enum OpCode
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
