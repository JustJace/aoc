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
            //var day1 = new D1.D1Solver();
            //PrintSolution("1.1", day1.SolveP1);
            //PrintSolution("1.2", day1.SolveP2);

            //var day2 = new D2.D2Solver();
            //PrintSolution("2.1", day2.SolveP1);
            //PrintSolution("2.2", day2.SolveP2);

            //var day3 = new D3.D3Solver();
            //PrintSolution("3.1", day3.SolveP1);
            //PrintSolution("3.2", day3.SolveP2);

            //var day4 = new D4.D4Solver();
            //PrintSolution("4.1", day4.SolveP1);
            //PrintSolution("4.2", day4.SolveP2);

            //var day5 = new D5.D5Solver();
            //PrintSolution("5.1", day5.SolveP1);
            //PrintSolution("5.2", day5.SolveP2);

            //var day6 = new D6.D6Solver();
            //PrintSolution("6.1", day6.SolveP1);
            //PrintSolution("6.2", day6.SolveP2);

            var day7 = new D7.D7Solver();
            PrintSolution("7.1", day7.SolveP1);
            PrintSolution("7.2", day7.SolveP2);


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
