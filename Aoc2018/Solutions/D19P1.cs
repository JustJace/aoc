using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Aoc2018.Solutions
{
    public class D19P1 : Solver<int>
    {
        public override int Day => 19;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d19.input";

        protected override int GetAnswer(string input)
        {
            var (pointer, instructions) = ParseInput(input);

            for (int i = 0; i < instructions.Count; i++)
                Console.WriteLine($"{i}: {instructions[i]}");
            //Console.In.ReadLine();

            var registers = new Registers();
            registers[0] = 1;

            while (registers[pointer] >= 0 && registers[pointer] < instructions.Count)
            {
                //Console.WriteLine(instructions[(int)registers[pointer]]);

                registers = RunOp(registers, instructions[registers[pointer]]);
                registers[pointer]++;

                //Console.WriteLine(registers);
                //Console.In.ReadLine();
            }

            return (int)registers[0];

            return Run();
        }

        private int Run()
        {
            var registers = new Registers();
            registers[2] = 2 * 2 * 19 * 11;
            registers[3] = 6 * 22 + 9;
            registers[2] += registers[3];
            registers[3] = (27 * 28 + 29) * 30 * 14 * 32;
            registers[2] += registers[3];

            for (var i = 1; i <= registers[2]; i++)
            {
                if (registers[2] % i == 0)
                    registers[0] += i;
            }

            return registers[0];


            while (registers[4] <= registers[2])
            {
                while (registers[1] <= registers[2])
                {
                    registers[1]++;
                    registers[3] = registers[1] * registers[4];
                    if (registers[2] == registers[3])
                        registers[0] += registers[4];
                }

                registers[4]++;

                registers[1] = 1;

                registers[3] = registers[1] * registers[4];
                if (registers[2] == registers[3])
                    registers[0] += registers[4];
            }

            return registers[0];
        }

        /*
0:  addi 5 16 5 -- [5]+=16
1:  seti 1 0 4  -- [4]=1
2:  seti 1 8 1  -- [1]=1
3:  mulr 4 1 3  -- [3]=[1]*[4]
4:  eqrr 3 2 3  -- [3]=[2]==[3]?1:0
5:  addr 3 5 5  -- [5]+=[3]
6:  addi 5 1 5  -- [5]+=1
7:  addr 4 0 0  -- [0]+=[4]
8:  addi 1 1 1  -- [1]+=1
9:  gtrr 1 2 3  -- [3]=[1]>[2]?1:0
10: addr 5 3 5  -- [5]+=[3]
11: seti 2 4 5  -- [5]=2
12: addi 4 1 4  -- [4]+=1
13: gtrr 4 2 3  -- [3]=[4]>[2]?1:0
14: addr 3 5 5  -- [5]+=[3]
15: seti 1 7 5  -- [5]=1
16: mulr 5 5 5  -- [5]*=[5]
17: addi 2 2 2  -- [2]+=2
18: mulr 2 2 2  -- [2]*=[2]
19: mulr 5 2 2  -- [2]*=[5]
20: muli 2 11 2 -- [2]*=11
21: addi 3 6 3  -- [3]+=6
22: mulr 3 5 3  -- [3]*=[5]
23: addi 3 9 3  -- [3]+=9
24: addr 2 3 2  -- [2]+=[3]
25: addr 5 0 5  -- [5]+=[0]
26: seti 0 5 5  -- [5]=0
27: setr 5 9 3  -- [3]=[5]
28: mulr 3 5 3  -- [3]*=[5]
29: addr 5 3 3  -- [3]+=[5]
30: mulr 5 3 3  -- [3]*=[5]
31: muli 3 14 3 -- [3]*=14
32: mulr 3 5 3  -- [3]*=[5]
33: addr 2 3 2  -- [2]+=[3]
34: seti 0 1 0  -- [0]=0
35: seti 0 0 5  -- [5]=0
*/

        private Registers RunOp(Registers registers, Instruction instruction)
        {
            switch (instruction.OpCode)
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

        private Tuple<int, List<Instruction>> ParseInput(string input)
        {
            var lines = input.Split(Environment.NewLine);
            var pointerRegex = new Regex(@"#ip (\d)");

            var pointer = int.Parse(pointerRegex.Match(lines[0]).Groups[1].ToString());
            var instructions = lines
                .Skip(1)
                .Select(ParseInstruction)
                .ToList();

            return new Tuple<int, List<Instruction>>(pointer, instructions);
        }

        private static readonly Regex _instructionRegex = new Regex(@"(.+) (\d+) (\d+) (\d+)");
        private Instruction ParseInstruction(string line)
        {
            var instruction = new Instruction();
            var match = _instructionRegex.Match(line);
            instruction.OpCode = (OpCode)Enum.Parse(typeof(OpCode), match.Groups[1].ToString());
            instruction.A = int.Parse(match.Groups[2].ToString());
            instruction.B = int.Parse(match.Groups[3].ToString());
            instruction.C = int.Parse(match.Groups[4].ToString());
            return instruction;
        }

        private class Instruction
        {
            public OpCode OpCode { get; set; }
            public int A { get; set; }
            public int B { get; set; }
            public int C { get; set; }
            public override string ToString()
            {
                return $"{OpCode} {A} {B} {C}";
            }
        }

        private class Registers
        {
            public Registers Clone() => MemberwiseClone() as Registers;
            public static bool Equal(Registers r1, Registers r2)
            {
                return r1[0] == r2[0]
                    && r1[1] == r2[1]
                    && r1[2] == r2[2]
                    && r1[3] == r2[3]
                    && r1[4] == r2[4]
                    && r1[5] == r2[5];
            }
            public int R0 { get; set; }
            public int R1 { get; set; }
            public int R2 { get; set; }
            public int R3 { get; set; }
            public int R4 { get; set; }
            public int R5 { get; set; }
            public int R6 { get; set; }
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
                        case 4: return R4;
                        case 5: return R5;
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
                        case 4: R4 = value; break;
                        case 5: R5 = value; break;
                        default: throw new Exception("Trying to access register that doesn't exist.");
                    }
                }
            }

            public override string ToString()
            {
                return $"{R0}, {R1}, {R2}, {R3}, {R4}, {R5}";
            }
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
