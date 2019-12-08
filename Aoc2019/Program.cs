using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Aoc2019
{
    class Program
    {
        static Stopwatch _stopwatch = new Stopwatch();

        static void Main(string[] args)
        {
            var solvers = new List<ISolve>();
            foreach (var type in Assembly.GetEntryAssembly().GetTypes())
            {
                if (!type.IsAbstract && typeof(ISolve).IsAssignableFrom(type))
                {
                    solvers.Add(Activator.CreateInstance(type) as ISolve);
                }
            }

            //SolveExactly(solvers, 5, 1);
            SolveMostRecent(solvers);
            //SolveAll(solvers);

            Console.In.ReadLine();
        }

        private static void SolveExactly(List<ISolve> solvers, int day, int part)
        {
            foreach (var solver in solvers.Where(s => s.Day == day && s.Part == part))
            {
                TimedSolvesWithPrint(solver, 0);
            }
        }

        private static void SolveAll(List<ISolve> solvers)
        {
            foreach (var solver in solvers.OrderBy(s => s.Day).ThenBy(s => s.Part))
            {
                TimedSolvesWithPrint(solver);
            }
        }

        private static void SolveMostRecent(List<ISolve> solvers)
        {
            foreach (var solver in solvers
               .OrderByDescending(s => s.Day)
               .ThenByDescending(s => s.Part)
               .Take(1))
            {
                TimedSolvesWithPrint(solver);
            }
        }

        static void TimedSolvesWithPrint(ISolve solver, int averageOver = 10)
        {
            var answer = solver.Solve();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"--- {solver.Day}.{solver.Part} ---");
            Console.ResetColor();
            Console.Write("An: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(answer);
            Console.ResetColor();

            var times = new List<double>();

            for (var i = 0; i < averageOver; i++)
            {
                _stopwatch.Restart();
                solver.Solve();
                _stopwatch.Stop();

                times.Add(_stopwatch.Elapsed.TotalMilliseconds);
            }

            if (times.Any())
            {

                Console.WriteLine($@"
Hi: {times.Max():N3}ms
Lo: {times.Min():N3}ms
Av: {times.Average():N3}ms");
            }
        }
    }
}
