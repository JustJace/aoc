using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Aoc2018.Solutions
{
    public class D25P1 : Solver<int>
    {
        public override int Day => 25;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d25.input";

        protected override int GetAnswer(string input)
        {
            var pointRegex = new Regex(@"([0-9-]+),([0-9-]+),([0-9-]+),([0-9-]+)");
            var points = input
                .Split(Environment.NewLine)
                .Select(l => pointRegex.Match(l))
                .Select(m => new Point4
                (
                    int.Parse(m.Groups[1].ToString()),
                    int.Parse(m.Groups[2].ToString()),
                    int.Parse(m.Groups[3].ToString()),
                    int.Parse(m.Groups[4].ToString())
                ))
                .ToArray();

            var constellations = new List<List<Point4>>();

            foreach (var point in points)
            {
                var matchingConstellationIndicies = new List<int>();
                for (int i = 0; i < constellations.Count; i++)
                {
                    var constellation = constellations[i];
                    foreach (var otherPoint in constellation)
                    {
                        if (Manhatten4(point, otherPoint) <= 3)
                        {
                            matchingConstellationIndicies.Add(i);
                            break;
                        }
                    }
                }

                if (matchingConstellationIndicies.Count == 0)
                {
                    constellations.Add(new List<Point4>() { point });
                }
                else if (matchingConstellationIndicies.Count == 1)
                {
                    constellations[matchingConstellationIndicies.First()].Add(point);
                }
                else
                {
                    var combinedConstellation = new List<Point4>() { point };
                    foreach (var indice in matchingConstellationIndicies.OrderByDescending(i => i))
                    {
                        combinedConstellation.AddRange(constellations[indice]);
                        constellations.RemoveAt(indice);
                    }
                    constellations.Add(combinedConstellation);
                }
            }

            return constellations.Count;
        }

        // 361 too high

        private int Manhatten4(Point4 p1, Point4 p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y) + Math.Abs(p1.Z - p2.Z) + Math.Abs(p1.T - p2.T);
        }

        private class Point4
        {
            public Point4(int x, int y, int z, int t)
            {
                X = x;
                Y = y;
                Z = z;
                T = t;
                Hash = $"{x},{y},{z},{t}";
            }

            public int X { get; }
            public int Y { get; }
            public int Z { get; }
            public int T { get; }
            public string Hash { get; }
        }
    }
}