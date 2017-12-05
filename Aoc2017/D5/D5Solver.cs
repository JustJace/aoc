using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Aoc2017.D5
{
    public class D5Solver
    {
        public int SolveP1() => BruteForceP1(File.ReadAllText(@"D5\D5.input"));
        private int BruteForceP1(string input)
        {
            var jumps = input.Split(Environment.NewLine).Select(s => int.Parse(s)).ToArray();

            var jumpCount = 0;
            var nextInstruction = 0;

            while (nextInstruction >= 0 && nextInstruction < jumps.Length)
            {
                var currentInstruction = nextInstruction;
                nextInstruction += jumps[currentInstruction];
                jumps[currentInstruction]++;
                jumpCount++;
            }

            return jumpCount;
        }

        public int SolveP2() => BruteForceP2(File.ReadAllText(@"D5\D5.input"));
        private int BruteForceP2(string input)
        {
            var jumps = input.Split(Environment.NewLine).Select(s => int.Parse(s)).ToArray();

            var jumpCount = 0;
            var nextInstruction = 0;

            while (nextInstruction >= 0 && nextInstruction < jumps.Length)
            {
                var currentInstruction = nextInstruction;
                nextInstruction += jumps[currentInstruction];
                if (jumps[currentInstruction] >= 3)
                    jumps[currentInstruction]--;
                else
                    jumps[currentInstruction]++;
                jumpCount++;
            }

            return jumpCount;
        }
    }
}
