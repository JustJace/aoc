using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2018.Solutions
{
    public class D11P2 : Solver<Tuple<int, int, int>>
    {
        public override int Day => 11;

        public override int Part => 2;

        protected override string Filename => @"Inputs\d11.input";

        protected override Tuple<int, int, int> GetAnswer(string input)
        {
            var serialNumber = int.Parse(input);

            var grid = new Cell[300, 300];

            for (var y = 0; y < 300; y++)
                for (var x = 0; x < 300; x++)
                    grid[y, x] = new Cell(serialNumber, x, y);

            var best = 0;
            var bestCoordinate = new Tuple<int, int, int>(0, 0, 0);

            for (var y = 0; y < 300; y++)
            for (var x = 0; x < 300; x++)
            {
                var totalPower = grid[y,x].PowerLevel;
                for (var s = 1; s < Math.Min(300 - x, 300 - y); s++)
                {
                    for (var dx = 0; dx < s + 1; dx++)
                        totalPower += grid[y + s, x + dx].PowerLevel;

                    for (var dy = 0; dy < s + 1; dy++)
                        totalPower += grid[y + dy, x + s].PowerLevel;

                    totalPower -= grid[y + s, x + s].PowerLevel;

                    if (totalPower > best)
                    {
                        best = totalPower;
                        bestCoordinate = new Tuple<int, int, int>(x, y, s + 1);
                    }
                }
            }

            return bestCoordinate;
        }

        private class Cell
        {
            public Cell(int serial, int x, int y)
            {
                PowerLevel = (((x + 10) * y + serial) * (x + 10)) / 100 % 10 - 5;
            }

            public int PowerLevel { get; }
        }
    }
}
