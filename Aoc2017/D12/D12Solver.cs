using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Aoc2017.D12
{
    public class D12Solver
    {
        class Program
        {
            public int Moniker { get; set; }
            public List<Program> PipesTo { get; } = new List<Program>();
            public void AddPipe(Program p)
            {
                if (!PipesTo.Contains(p))
                    PipesTo.Add(p);
            }
        }

        public int SolveP1()
        {
            var programs = ParsePrograms();
            var root = programs[0];
            var seen = new HashSet<int>() { 0 };

            DFS(root, seen);

            return seen.Count;
        }

        private Dictionary<int, Program> ParsePrograms()
        {
            var programData = File.ReadAllText(@"D12\D12.input").Split(Environment.NewLine);
            var programs = new Dictionary<int, Program>();
            var programRegex = new Regex(@"([0-9]+) <-> (.*)");

            foreach (var programDatum in programData)
            {
                var match = programRegex.Match(programDatum);
                var moniker = int.Parse(match.Groups[1].ToString());
                var pipesTo = match.Groups[2].ToString().Split(',').Select(s => s.Trim()).Select(int.Parse);

                if (!programs.ContainsKey(moniker))
                    programs[moniker] = new Program() { Moniker = moniker };

                foreach (var pipeTo in pipesTo)
                {
                    if (!programs.ContainsKey(pipeTo))
                        programs[pipeTo] = new Program() { Moniker = pipeTo };

                    programs[moniker].AddPipe(programs[pipeTo]);
                    programs[pipeTo].AddPipe(programs[moniker]);
                }
            }

            return programs;
        }

        private void DFS(Program node, HashSet<int> seen)
        {
            foreach (var child in node.PipesTo)
            {
                if (!seen.Contains(child.Moniker))
                {
                    seen.Add(child.Moniker);
                    DFS(child, seen);
                }
            }
        }

        public int SolveP2()
        {
            var programs = ParsePrograms();
            var seen = new HashSet<int>();
            var groupCount = 0;

            while (seen.Count != programs.Count)
            {
                var root = programs.First(p => !seen.Contains(p.Key)).Value;

                seen.Add(root.Moniker);

                DFS(root, seen);

                groupCount++;
            }

            return groupCount;
        }
    }
}
