using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2018.Solutions
{
    public class D6P1 : Solver<int>
    {
        public override int Day => 6;
        public override int Part => 1;
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
            {
                for (var y = 0; y < maxY; y++)
                {
                    var distances = new List<int>();

                    for (var z = 0; z < points.Length; z++)
                    {
                        var p = points[z];
                        distances.Add(ManhattenDistance(x, y, p.X, p.Y));
                    }

                    var minDistance = distances.Min();

                    if (distances.Count(d => d == minDistance) > 1)
                    {
                        map[x, y] = -1;
                    }
                    else
                    {
                        map[x, y] = distances.IndexOf(minDistance);
                    }
                }
            }

            //using (var fs = new FileStream("d6p1.output", FileMode.CreateNew))
            //using (var sw = new StreamWriter(fs))
            //{
            //    for (var x = 0; x < maxX; x++)
            //    {
            //        for (var y = 0; y < maxY; y++)
            //        {
            //            sw.Write($"{map[x, y]}".PadRight(5));
            //        }

            //        sw.WriteLine();
            //    }
            //}

            var best = 0;

            for (var z = 0; z < points.Length; z++)
            {
                var areaPoints = 0;
                var onEdge = false;
                for (var x = 0; x < maxX; x++)
                {
                    for (var y = 0; y < maxY; y++)
                    {
                        if (map[x, y] == z)
                        {
                            areaPoints++;

                            if (x == 0 || y == 0 || x == maxX - 1 || y == maxY - 1)
                            {
                                onEdge = true;
                                break;
                            }
                        }
                    }

                    if (onEdge)
                        break;
                }

                if (onEdge)
                    continue;

                if (areaPoints > best)
                    best = areaPoints;
            }

            return best;
        }

        private int ManhattenDistance(int x1, int y1, int x2, int y2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);

        private class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
