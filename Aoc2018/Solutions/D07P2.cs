using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2018.Solutions
{
    public class D07P2 : Solver<int>
    {
        public override int Day => 7;
        public override int Part => 2;
        protected override string Filename => @"Inputs\d07.input";

        protected override int GetAnswer(string input)
        {
            var timeIncrease = 60;
            var workerCount = 5;

            var regex = new Regex(@"Step ([A-Z]) must be finished before step ([A-Z]) can begin\.");
            var instructions = input
                .Split(Environment.NewLine)
                .Select(i => regex.Match(i))
                .Select(m => new Instruction
                {
                    PrerequisiteStepId = m.Groups[1].ToString(),
                    StepId = m.Groups[2].ToString()
                });

            var steps = new Dictionary<string, Step>();

            foreach (var instruction in instructions)
            {
                if (!steps.ContainsKey(instruction.StepId))
                    steps[instruction.StepId] = new Step
                    {
                        StepId = instruction.StepId,
                        TimeLeft = timeIncrease + (int)instruction.StepId[0] - 64
                    };

                if (!steps.ContainsKey(instruction.PrerequisiteStepId))
                    steps[instruction.PrerequisiteStepId] = new Step
                    {
                        StepId = instruction.PrerequisiteStepId,
                        TimeLeft = timeIncrease + (int)instruction.PrerequisiteStepId[0] - 64
                    };

                steps[instruction.StepId].PrerequisiteStepIds.Add(instruction.PrerequisiteStepId);
                steps[instruction.PrerequisiteStepId].DescendantStepIds.Add(instruction.StepId);
            }

            var elves = new string[workerCount];

            var startStep = steps.Values.First(s => s.PrerequisiteStepIds.Count == 0);
            var unstartedStepIds = steps.Values.Select(s => s.StepId).OrderBy(c => c).ToList();
            var completedStepIds = "";
            var totalSeconds = -1;

            while (completedStepIds.Length < steps.Count)
            {
                for (var e = 0; e < elves.Length; e++)
                {
                    if (!string.IsNullOrEmpty(elves[e]))
                    {
                        steps[elves[e]].TimeLeft--;

                        if (steps[elves[e]].TimeLeft == 0)
                        {
                            completedStepIds += elves[e];
                            elves[e] = null;
                        }
                    }
                }

                for ( var e = 0; e < elves.Length; e++)
                {
                    if (string.IsNullOrEmpty(elves[e]))
                    {
                        var nextStepId = unstartedStepIds.FirstOrDefault(s => IsSatisified(steps, completedStepIds, s));
                        if (!string.IsNullOrEmpty(nextStepId))
                        {
                            unstartedStepIds.Remove(nextStepId);
                            elves[e] = nextStepId;
                        }
                    }
                }

                totalSeconds++;

                //Console.Write($"{totalSeconds.ToString().PadLeft(4)}s - ");
                //for(var e = 0; e < workerCount; e++)
                //{
                //    if (string.IsNullOrEmpty(elves[e]))
                //    {
                //        Console.Write("   .  ");
                //    }
                //    else
                //    {
                //        Console.Write($"{elves[e]}:{steps[elves[e]].TimeLeft.ToString().PadLeft(2)}  ");
                //    }
                //}
                //Console.Write($" - {completedStepIds}");
                //Console.WriteLine();

            }

            return totalSeconds;
        }

        private bool IsSatisified(Dictionary<string, Step> steps, string completedStepIds, string testId)
        {
            var satisfied = true;

            foreach (var pStepId in steps[testId].PrerequisiteStepIds)
            {
                if (!completedStepIds.Contains(pStepId))
                {
                    satisfied = false;
                    break;
                }
            }

            return satisfied;
        }

        private class Step
        {
            public string StepId { get; set; }
            public List<string> PrerequisiteStepIds { get; } = new List<string>();
            public List<string> DescendantStepIds { get; } = new List<string>();
            public int TimeLeft { get; set; }
        }

        private class Instruction
        {
            public string StepId { get; set; }
            public string PrerequisiteStepId { get; set; }
        }
    }
}
