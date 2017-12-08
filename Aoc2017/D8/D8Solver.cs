using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Aoc2017.D8
{
    enum Command
    {
        Inc,
        Dec
    }
    enum ConditionOperator
    {
        EQ,
        GT,
        LT,
        GTE,
        LTE,
        NE
    }
    class Instruction
    {
        public int Ordinal { get; set; }
        public string ModifyRegister { get; set; }
        public Command Command { get; set; }
        public string ModifyAmount { get; set; }
        public string ConditionRegister { get; set; }
        public ConditionOperator ConditionOperator { get; set; }
        public string ConditionAmount { get; set; }

    }
    public class D8Solver
    {
        public int SolveP1() => ProcessInstructionsP1(File.ReadAllText(@"D8\D8.input"));
        private int ProcessInstructionsP1(string input)
        {
            var instructions = ParseInstructions(input);
            var registers = new Dictionary<string, int>();

            foreach (var instruction in instructions)
            {
                if (TestCondition(registers, instruction))
                    RunCommand(registers, instruction);
            }

            return registers.Values.Max();
        }

        public int SolveP2() => ProcessInstructionsP2(File.ReadAllText(@"D8\D8.input"));
        private int ProcessInstructionsP2(string input)
        {
            var instructions = ParseInstructions(input);
            var registers = new Dictionary<string, int>();
            var bestMax = 0;

            foreach (var instruction in instructions)
            {
                if (TestCondition(registers, instruction))
                {
                    RunCommand(registers, instruction);

                    var newMax = registers.Values.Max();
                    if (newMax > bestMax)
                        bestMax = newMax;
                }
            }

            return bestMax;
        }

        // This was unnecessary for this problem... but last year it was a thing
        private int RegisterOrValue(Dictionary<string, int> registers, string registerOrValue)
        {
            if (!int.TryParse(registerOrValue, out int value))
            {
                if (!registers.ContainsKey(registerOrValue))
                    registers[registerOrValue] = 0;

                value = registers[registerOrValue];
            }

            return value;
        }

        private bool TestCondition(Dictionary<string, int> registers, Instruction instruction)
        {
            var left = RegisterOrValue(registers, instruction.ConditionRegister);
            var right = RegisterOrValue(registers, instruction.ConditionAmount);

            switch (instruction.ConditionOperator)
            {
                case ConditionOperator.EQ: return left == right;
                case ConditionOperator.GT: return left > right;
                case ConditionOperator.GTE: return left >= right;
                case ConditionOperator.LT: return left < right;
                case ConditionOperator.LTE: return left <= right;
                case ConditionOperator.NE: return left != right;
                default: throw new Exception("You messed up. Couldn't test condition.");
            }
        }

        private void RunCommand(Dictionary<string, int> registers, Instruction instruction)
        {
            if (!registers.ContainsKey(instruction.ModifyRegister))
                registers[instruction.ModifyRegister] = 0;

            var modifyAmount = RegisterOrValue(registers, instruction.ModifyAmount);

            switch (instruction.Command)
            {
                case Command.Inc:
                    registers[instruction.ModifyRegister] += modifyAmount;
                    break;
                case Command.Dec:
                    registers[instruction.ModifyRegister] -= modifyAmount;
                    break;
                default:
                    throw new Exception("You messed up. Couldn't test condition.");
            }
        }

        private List<Instruction> ParseInstructions(string instructionInput)
        {
            var instructionData = instructionInput.Split(Environment.NewLine);
            var instructions = new List<Instruction>();
            var instructionRegex = new Regex(@"(.*) (.*) (.*) if (.*) (.*) (.*)");

            for (var i = 0; i < instructionData.Length; i++)
            {
                var match = instructionRegex.Match(instructionData[i]);
                instructions.Add(new Instruction
                {
                    Ordinal = i,
                    ModifyRegister = match.Groups[1].ToString(),
                    Command = ParseCommand(match.Groups[2].ToString()),
                    ModifyAmount = match.Groups[3].ToString(),
                    ConditionRegister = match.Groups[4].ToString(),
                    ConditionOperator = ParseOperator(match.Groups[5].ToString()),
                    ConditionAmount = match.Groups[6].ToString()
                });
            }
            return instructions;
        }

        private ConditionOperator ParseOperator(string operatorText)
        {
            switch (operatorText)
            {
                case "==": return ConditionOperator.EQ;
                case ">": return ConditionOperator.GT;
                case "<": return ConditionOperator.LT;
                case ">=": return ConditionOperator.GTE;
                case "<=": return ConditionOperator.LTE;
                case "!=": return ConditionOperator.NE;
                default: throw new Exception("You messed something up. Couldn't parse operator.");
            }
        }

        private Command ParseCommand(string commandText)
        {
            switch (commandText.ToLower())
            {
                case "inc":
                    return Command.Inc;
                case "dec":
                    return Command.Dec;
                default:
                    throw new Exception("You messed something up. Couldn't parse command.");
            }
        }
    }
}
