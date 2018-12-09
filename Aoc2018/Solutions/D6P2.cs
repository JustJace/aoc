using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2018.Solutions
{
    public class D6P2 : Solver<int>
    {
        public override int Day => 6;
        public override int Part => 2;
        protected override string Filename => @"Inputs\d6p1.input";

        protected override int GetAnswer(string input)
        {
            var regex = new Regex(@"(\d+), (\d+)");
            var points = input
                .Split(Environment.NewLine)
                .Select(i => regex.Match(i))
                .Select(m => new Point
                {
                    X = int.Parse(m.Groups[1].ToString()),
                    Y = int.Parse(m.Groups[2].ToString())
                })
                .ToArray();

            var maxX = points.Select(p => p.X).Max();
            var maxY = points.Select(p => p.Y).Max();

            var map = new int[maxX, maxY];

            for (var x = 0; x < maxX; x++)
                for (var y = 0; y < maxY; y++)
                    for (var p = 0; p < points.Length; p++)
                        map[x, y] += ManhattenDistance(x, y, points[p].X, points[p].Y);

            var area = 0;

            for (var x = 0; x < maxX; x++)
                for (var y = 0; y < maxY; y++)
                    if (map[x, y] < 10000)
                        area++;

            return area;
        }

        private int ManhattenDistance(int x1, int y1, int x2, int y2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);

        private class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
