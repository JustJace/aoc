using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Aoc2017.D2
{
    public class D2Solver
    {
        public int SolveP1() => BruteForceP1(File.ReadAllText(@"D2\D2.input"));
        internal int BruteForceP1(string input)
        {
            var numbers = input.Split(Environment.NewLine)
                               .Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(n => int.Parse(n))
                                                   .ToArray())
                               .ToArray();

            var checksum = 0;

            for (var y = 0; y < numbers.Length; y++)
            {
                var max = numbers[y][0];
                var min = numbers[y][0];

                for (var x = 1; x < numbers[y].Length; x++)
                {
                    if (numbers[y][x] > max)
                        max = numbers[y][x];
                    else if (numbers[y][x] < min)
                        min = numbers[y][x];
                }

                checksum += max - min;
            }

            return checksum;
        }

        public int SolveP2() => BruteForceP2(File.ReadAllText(@"D2\D2.input"));
        internal int BruteForceP2(string input)
        {
            var numbers = input.Split(Environment.NewLine)
                              .Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(n => int.Parse(n))
                                                  .ToArray())
                              .ToArray();

            var checksum = 0;

           

            for (var y = 0; y < numbers.Length; y++)
            {
                int? numerator = null;
                int? denominator = null;

                for (var xa = 0; xa < numbers[y].Length; xa++)
                {
                    for (var xb = 0; xb < numbers[y].Length; xb++)
                    {
                        if (xa == xb) continue;

                        if (numbers[y][xa] % numbers[y][xb] == 0)
                        {
                            numerator = numbers[y][xa];
                            denominator = numbers[y][xb];
                            break;
                        }
                        else if (numbers[y][xb] % numbers[y][xa] == 0)
                        {
                            numerator = numbers[y][xb];
                            denominator = numbers[y][xa];
                            break;
                        }
                    }

                    if (numerator.HasValue && denominator.HasValue)
                        break;
                }

                checksum += numerator.Value / denominator.Value;
            }

            return checksum;
        }
    }
}
