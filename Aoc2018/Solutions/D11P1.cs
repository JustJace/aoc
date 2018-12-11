using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2018.Solutions
{
    public class D11P1 : Solver<Tuple<int, int>>
    {
        public override int Day => 11;

        public override int Part => 1;

        protected override string Filename => @"Inputs\d11.input";

        protected override Tuple<int, int> GetAnswer(string input)
        {
            var serialNumber = int.Parse(input);

            var grid = new Cell[300, 300];

            for (var y = 0; y < 300; y++)
                for (var x = 0; x < 300; x++)
                    grid[y, x] = new Cell(serialNumber, x, y);

            var best = 0;
            var bestCoordinate = new Tuple<int, int>(0, 0);
            for (var y = 0; y < 300 - 2; y++)
            for (var x = 0; x < 300 - 2; x++)
            {
                    var totalPower = 0;
                    for (var dy = 0; dy < 3; dy++)
                        for (var dx = 0; dx < 3; dx++)
                            totalPower += grid[y + dy, x + dx].PowerLevel;

                    if (totalPower > best)
                    {
                        best = totalPower;
                        bestCoordinate = new Tuple<int, int>(x, y);
                    }
            }

            return bestCoordinate;
        }

        private class Cell
        {
            public Cell(int serial, int x, int y)
            {
                Serial = serial;
                X = x;
                Y = y;
            }
            public int Serial { get; }
            public int X { get; set; }
            public int Y { get; set; }
            public int RackId => X + 10;
            public int PowerLevel
            {
                get
                {
                    var p = (RackId * Y + Serial) * RackId;
                    p = int.Parse(p.ToString().PadLeft(3, '0').Reverse().ToArray()[2].ToString());
                    return p - 5;
                }
            }
        }
    }
}
