using System;
using System.Collections.Generic;
using System.Text;

namespace Aoc2017.D3
{
    public class D3Solver
    {
        public int SolveP1() => EstimateP1(347991);
        internal int EstimateP1(int input)
        {
            if (input == 1)
                return 0;

            var sideLength = 1;

            while (sideLength * sideLength < input)
                sideLength+=2;

            var closestCorner = sideLength * sideLength;

            while (closestCorner - input >= sideLength / 2)
                closestCorner -= sideLength - 1;

            var distanceToCorner = Math.Abs(closestCorner - input);


            var manhatten = sideLength - 1 - distanceToCorner;

            return manhatten;
        }

        public int SolveP2() => BruteForceP2(347991);
        internal int BruteForceP2(int input)
        {
            if (input == 1)
                return 2;

            var sideLength = 1;

            while (sideLength * sideLength < input)
                sideLength += 2;
            sideLength += 4; // Just to make checking array bounds unnecessary

            var numbers = new int[sideLength, sideLength];
            var x = sideLength / 2 + 1;
            var y = sideLength / 2 + 1;
            numbers[y, x] = 1;
            x++;

            for (var currentSide = 3; currentSide <= sideLength; currentSide += 2)
            {
                // Go up sidelength - 1
                for (var i = 0; i < currentSide - 1; i++)
                {
                    var value = ComputeSquareValue(numbers, y, x);
                    if (value > input) return value;
                    numbers[y, x] = value;
                    y--;
                }
                y++; x--;

                // Go left sidelength - 1
                for (var i = 0; i < currentSide - 1; i++)
                {
                    var value = ComputeSquareValue(numbers, y, x);
                    if (value > input) return value;
                    numbers[y, x] = value;
                    x--;
                }
                y++; x++;

                // Go down sidelength - 1
                for (var i = 0; i < currentSide - 1; i++)
                {
                    var value = ComputeSquareValue(numbers, y, x);
                    if (value > input) return value;
                    numbers[y, x] = value;
                    y++;
                }
                y--; x++;
                // Go right sidelength - 1
                for (var i = 0; i < currentSide - 1; i++)
                {
                    var value = ComputeSquareValue(numbers, y, x);
                    if (value > input) return value;
                    numbers[y, x] = value;
                    x++;
                }
            }

            throw new Exception("We should've found the answer within the given bounds");
        }

        private int ComputeSquareValue(int[,] numbers, int y, int x)
        {
            return numbers[y - 1, x - 1]
                 + numbers[y - 1, x]
                 + numbers[y - 1, x + 1]
                 + numbers[y, x - 1]
                 + numbers[y, x + 1]
                 + numbers[y + 1, x - 1]
                 + numbers[y + 1, x]
                 + numbers[y + 1, x + 1];
        }

        private string NumbersToString(int[,] numbers)
        {
            var s = "";

            for (var y = 0; y < numbers.GetLength(0); y++)
            {
                for (var x = 0; x < numbers.GetLength(1); x++)
                {
                    s += numbers[y, x];

                    if (x != numbers.GetLength(1))
                        s += ' ';
                }

                if (y != numbers.GetLength(0))
                    s += Environment.NewLine;
            }

            return s;
        }
    }
}
