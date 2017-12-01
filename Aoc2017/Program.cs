using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Aoc2017.Tests")]

namespace Aoc2017
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeAndPrintSolution("1.1", D1.D1Solver.SolveP1);
            TimeAndPrintSolution("1.2", D1.D1Solver.SolveP2);

            Console.ReadLine();
        }

        static void TimeAndPrintSolution<T>(string moniker, Func<T> solveFn)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var answer = solveFn();

            stopwatch.Stop();

            Console.WriteLine($"{moniker}: {answer} in {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
