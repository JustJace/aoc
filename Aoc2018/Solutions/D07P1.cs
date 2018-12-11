using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2018.Solutions
{
    public class D07P1 : Solver<string>
    {
        public override int Day => 7;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d07.input";

        protected override string GetAnswer(string input)
        {
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
                    steps[instruction.StepId] = new Step { StepId = instruction.StepId };

                if (!steps.ContainsKey(instruction.PrerequisiteStepId))
                    steps[instruction.PrerequisiteStepId] = new Step { StepId = instruction.PrerequisiteStepId };

                steps[instruction.StepId].PrerequisiteStepIds.Add(instruction.PrerequisiteStepId);
                steps[instruction.PrerequisiteStepId].DescendantStepIds.Add(instruction.StepId);
            }

            var startStep = steps.Values.First(s => s.PrerequisiteStepIds.Count == 0);
            var unfinishedStepIds = steps.Values.Select(s => s.StepId).OrderBy(c => c).ToList();

            var completedStepIds = "";

            while (unfinishedStepIds.Any())
            {
                var nextStepId = unfinishedStepIds.First(s => IsSatisified(steps, completedStepIds, s));
                unfinishedStepIds.Remove(nextStepId);
                completedStepIds += nextStepId;
            }

            return completedStepIds;
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
        }

        private class Instruction
        {
            public string StepId { get; set; }
            public string PrerequisiteStepId { get; set; }
        }
    }
}
