using System;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Aoc2017.Tests")]

namespace Aoc2017
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"D1.P1: {D1.D1Solver.SolveP1()}");
            Console.WriteLine($"D1.P2: {D1.D1Solver.SolveP2()}");
            Console.ReadLine();
        }
    }
}
