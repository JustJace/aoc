using Aoc2018.Solutions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Aoc2018
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

            foreach (var solver in solvers
            //.OrderBy(s => s.Day).ThenBy(s => s.Part))
                .OrderByDescending(s => s.Day)
                .ThenByDescending(s => s.Part)
                .Take(1))
            {
                TimedSolvesWithPrint(solver);
            }
            
            Console.In.ReadLine();
        }


        static void TimedSolvesWithPrint(ISolve solver)
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

            for (var i = 0; i < 10; i++)
            {
                _stopwatch.Restart();
                solver.Solve();
                _stopwatch.Stop();

                times.Add(_stopwatch.Elapsed.TotalMilliseconds);
            }

            Console.WriteLine($@"
Hi: {times.Max():N3}ms
Lo: {times.Min():N3}ms
Av: {times.Average():N3}ms");
        }
    }
}
