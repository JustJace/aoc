using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Linq;
[assembly: InternalsVisibleTo("Aoc2017.Tests")]

namespace Aoc2017
{
    class Program
    {
        static Stopwatch _stopwatch = new Stopwatch();
        static void Main(string[] args)
        {
            var day1 = new D1.D1Solver();

            PrintSolution("1.1", day1.SolveP1);
            PrintSolution("1.2", day1.SolveP2);

            Console.ReadLine();
        }

        static void PrintSolution<T>(string moniker, Func<T> solveFn)
        {
            var answer = solveFn();

            var times = new List<double>();

            for (var i = 0; i < 10; i++)
            {
                _stopwatch.Restart();
                solveFn();
                _stopwatch.Stop();

                times.Add(_stopwatch.Elapsed.TotalMilliseconds);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"--- {moniker} ---");
            Console.ResetColor();
            Console.Write("An: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(answer);
            Console.ResetColor();
            Console.WriteLine($@"
Hi: {times.Max():N3}ms
Lo: {times.Min():N3}ms
Av: {times.Average():N3}ms");
        }
    }
}
