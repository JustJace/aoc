using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Aoc2018.Solutions
{
    public class D21P1 : Solver<int>
    {
        public override int Day => 21;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d21.input";

        protected override int GetAnswer(string input)
        {
            var (pointer, instructions) = ParseInput(input);

            return TestUntilInstruction(instructions, pointer, 28);
        }

        //private int Compute(int pointer, List<Instruction> instructions)
        //{
        //    var r = new Registers();

        //    r[1] = r[5] | 65536;
        //    r[5] = 10678677;

        //    while (true)
        //    {
        //        r[4] = r[1] | 255;
        //        r[5] += r[4];
        //        r[5] |= 16777215;
        //        r[5] *= 65899;
        //        r[5] |= 16777215;

        //        if (256 > r[1])
        //            break;

        //        r[4] = 0;
        //        while (true)
        //        {
        //            r[3] = r[4] + 1;
        //            r[3] *= 256;
        //            if (r[3] > r[1])
        //                break;

        //            r[4] += 1;
        //        }

        //        r[1] = r[4];
        //    }

        //    return r[5];
        //}

        private int TestUntilInstruction(List<Instruction> instructions, int pointer, int endOnInstruction)
        {
            var registers = new Registers();
            var seen = new HashSet<int>();
            var lastSeen = 0;

            while (registers[pointer] >= 0 && registers[pointer] < instructions.Count)
            {
                //Console.WriteLine(instructions[(int)registers[pointer]]);

                registers = RunOp(registers, instructions[registers[pointer]]);
                registers[pointer]++;

                //Console.WriteLine(registers);
                //Console.In.ReadLine();

                if (registers[pointer] == endOnInstruction)
                {
                    if (seen.Contains(registers[5]))
                    {
                        return lastSeen;
                    }
                    else
                    {
                        seen.Add(registers[5]);
                        lastSeen = registers[5];
                    }
                }
            }

            throw new Exception("Shouldn't actually end here");
        }

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

/*
 * Advent of Code[About][Events][Shop][Settings][Log Out]Jace Brewster 40*
   $year=2018;[Calendar][AoC++][Sponsors][Leaderboard][Stats]
Our sponsors help make Advent of Code possible:
Xebia - an international network of passionate technologists and craftspeople, dedicated to exploring and creating new frontiers in IT
--- Day 21: Chronal Conversion ---
You should have been watching where you were going, because as you wander the new North Pole base, you trip and fall into a very deep hole!

Just kidding. You're falling through time again.

If you keep up your current pace, you should have resolved all of the temporal anomalies by the next time the device activates. Since you have very little interest in browsing history in 500-year increments for the rest of your life, you need to find a way to get back to your present time.

After a little research, you discover two important facts about the behavior of the device:

First, you discover that the device is hard-wired to always send you back in time in 500-year increments. Changing this is probably not feasible.

Second, you discover the activation system (your puzzle input) for the time travel module. Currently, it appears to run forever without halting.

If you can cause the activation system to halt at a specific moment, maybe you can make the device send you so far back in time that you cause an integer underflow in time itself and wrap around back to your current time!

The device executes the program as specified in manual section one and manual section two.

Your goal is to figure out how the program works and cause it to halt. You can only control register 0; every other register begins at 0 as usual.

Because time travel is a dangerous activity, the activation system begins with a few instructions which verify that bitwise AND (via bani) does a numeric operation and not an operation as if the inputs were interpreted as strings. If the test fails, it enters an infinite loop re-running the test instead of allowing the program to execute normally. If the test passes, the program continues, and assumes that all other bitwise operations (banr, bori, and borr) also interpret their inputs as numbers. (Clearly, the Elves who wrote this system were worried that someone might introduce a bug while trying to emulate this system with a scripting language.)

What is the lowest non-negative integer value for register 0 that causes the program to halt after executing the fewest instructions? (Executing the same instruction multiple times counts as multiple instructions executed.)

To begin, get your puzzle input.

Answer: 
 


#ip 2
00      seti 123 0 5        -- [5]  = 123
01      bani 5 456 5        -- [5] &= 456 
02      eqri 5 72 5         -- [5]  = [5] == 72 ? 1 : 0
03      addr 5 2 2          -- [2] += [5]
04      seti 0 0 2          -- [2]  = 0
05      seti 0 4 5          -- [5]  = 0
06      bori 5 65536 1      -- [1]  = [5] | 65536
07      seti 10678677 3 5   -- [5]  = 10678677
08      bani 1 255 4        -- [4]  = [1] | 255
09      addr 5 4 5          -- [5] += [4]
10      bani 5 16777215 5   -- [5] |= 16777215
11      muli 5 65899 5      -- [5] *= 65899
12      bani 5 16777215 5   -- [5] |= 16777215
13      gtir 256 1 4        -- [4]  = 256 > [1] ? 1 : 0
14      addr 4 2 2          -- [2] += [4]
15      addi 2 1 2          -- [2] += 1
16      seti 27 5 2         -- [2]  = 27
17      seti 0 6 4          -- [4]  = 0
18      addi 4 1 3          -- [3]  = [4] + 1
19      muli 3 256 3        -- [3] *= 256
20      gtrr 3 1 3          -- [3]  = [3] > [1] ? 1 : 0
21      addr 3 2 2          -- [2] += [3]
22      addi 2 1 2          -- [2] += 1
23      seti 25 4 2         -- [2]  = 25
24      addi 4 1 4          -- [4] += 1
25      seti 17 6 2         -- [2]  = 17
26      setr 4 6 1          -- [1]  = [4]
27      seti 7 5 2          -- [2]  = 7
28      eqrr 5 0 4          -- [4]  = [5] == [0] : 1 : 0
29      addr 4 2 2          -- [2] += [4]
30      seti 5 4 2          -- [2]  = [5]




    */
