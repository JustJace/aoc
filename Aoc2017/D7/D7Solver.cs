using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Aoc2017.D7
{
    class Program
    {
        public string Moniker { get; set; }
        public Program Parent { get; set; }
        public int Weight { get; set; }
        public int StackWeight { get; set; }
        public int TotalWeight => Weight + StackWeight;
        public List<Program> Children { get; } = new List<Program>();
        public bool IsBalanced { get; set; }
        public override string ToString()
        {
            return $"{Moniker} | {TotalWeight} | {IsBalanced}";
        }
    }

    public class D7Solver
    {
        public string SolveP1() => TreeAndTraverseUp(File.ReadAllText(@"D7\D7.input"));
        private string TreeAndTraverseUp(string input) => FindRoot(ParseAndBuildTree(input));

        public int SolveP2() => BruteForceP2(File.ReadAllText(@"D7\D7.input"));
        private int BruteForceP2(string input)
        {
            var programs = ParseAndBuildTree(input);
            var root = programs[FindRoot(programs)];

            RecursivelySetStackWeightsAndBalanced(root);

            var imbalance = FindHighestImbalancedProgram(root);
            var problemChild = FindProblemChild(imbalance);
            var problemSibling = ProgramSibling(problemChild);
            var weightAdjust = problemSibling.TotalWeight - problemChild.TotalWeight;

            return problemChild.Weight + weightAdjust;
        }

        private Program ProgramSibling(Program problemChild) => problemChild.Parent.Children.Where(c => c.Moniker != problemChild.Moniker).First();

        private Program FindHighestImbalancedProgram(Program root)
        {
            var current = root;
            while (current.Children.Any(c => !c.IsBalanced))
                current = current.Children.First(c => !c.IsBalanced);

            return current;
        }

        private Program FindProblemChild(Program parent)
        {
            for (var i = 0; i < parent.Children.Count; i++)
            {
                var hasMatch = false;
                for (var j = 0; j < parent.Children.Count; j++)
                {
                    if (i == j) continue;

                    if (parent.Children[i].TotalWeight == parent.Children[j].TotalWeight)
                        hasMatch = true;
                }

                if (!hasMatch)
                    return parent.Children[i];
            }

            throw new Exception("You told me there was a problem child, but I don't see it.");
        }

        private void RecursivelySetStackWeightsAndBalanced(Program program)
        {
            foreach (var child in program.Children)
                RecursivelySetStackWeightsAndBalanced(child);

            program.StackWeight = program.Children.Sum(c => c.TotalWeight);
            program.IsBalanced = program.Children.Count == 0 
                              || program.Children.Select(c => c.TotalWeight)
                                                 .Distinct()
                                                 .Count() == 1;
        }

        private Dictionary<string, Program> ParseAndBuildTree(string input)
        {
            var programs = new Dictionary<string, Program>();
            var programData = input.Split(Environment.NewLine);
            var programRegex = new Regex(@"([a-z]+) \(([0-9]+)\)");
            var discRegex = new Regex(@"[a-z]+ \([0-9]+\) -> (.*)");

            foreach (var programDatum in programData)
            {
                var programMatch = programRegex.Match(programDatum);
                var moniker = programMatch.Groups[1].ToString();

                if (!programs.ContainsKey(moniker))
                    programs[moniker] = new Program { Moniker = moniker };

                programs[moniker].Weight = int.Parse(programMatch.Groups[2].ToString());

                if (discRegex.IsMatch(programDatum))
                {
                    var discMatch = discRegex.Match(programDatum);
                    var childrenMonikers = discMatch.Groups[1].ToString().Split(',').Select(c => c.Trim());

                    foreach (var childMoniker in childrenMonikers)
                    {
                        if (!programs.ContainsKey(childMoniker))
                            programs[childMoniker] = new Program { Moniker = childMoniker };

                        programs[childMoniker].Parent = programs[moniker];
                        programs[moniker].Children.Add(programs[childMoniker]);
                    }
                }
            }

            return programs;
        }

        private string FindRoot(Dictionary<string, Program> programs)
        {
            var navProgram = programs.Values.First(p => p.Parent != null);

            while (navProgram.Parent != null)
                navProgram = navProgram.Parent;

            return navProgram.Moniker;
        }
    }
}
