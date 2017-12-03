using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace Aoc2017.D1
{
    public class D1Solver
    {
        public int SolveP1() => BruteForceP1(File.ReadAllText(@"D1\D1.input"));
        internal int BruteForceP1(string input)
        {
            var numbers = input.ToCharArray()
                               .Select(c => int.Parse(c.ToString()))
                               .ToList();

            numbers.Add(numbers[0]); // let's be honest, it'll be easier

            var sum = 0;

            for (var i = 0; i < numbers.Count - 1; i++)
                if (numbers[i] == numbers[i + 1])
                    sum += numbers[i];

            return sum;
        }

        public int SolveP2() => BruteForceP2(File.ReadAllText(@"D1\D1.input"));
        internal int BruteForceP2(string input)
        {
            var numbers = input.ToCharArray()
                               .Select(c => int.Parse(c.ToString()))
                               .ToList();

            var sum = 0;
            var diff = numbers.Count / 2;

            for (var i = 0; i < numbers.Count; i++)
                if (numbers[i] == numbers[(i + diff) % numbers.Count])
                    sum += numbers[i];

            return sum;
        }
    }
}
