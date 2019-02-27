using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Aoc2018.Solutions
{
    public class D22P1 : Solver<int>
    {
        public override int Day => 22;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d22.input";

        protected override int GetAnswer(string input)
        {
            var lines = input.Split(Environment.NewLine);
            var depth = int.Parse(lines[0].Split(' ')[1]);
            var targetXY = lines[1].Split(' ')[1].Split(',').Select(int.Parse).ToArray();
            var targetX = targetXY[0];
            var targetY = targetXY[1];

            var points = new Dictionary<string, Point>();
            points[$"0,0"] = new Point(0, 0, depth, ref points);
            points[$"{targetX},{targetY}"] = new Point(targetX, targetY, depth, ref points, true);

            var totalRisk = 0;
            foreach (var point in points.Values)
                totalRisk += point.Type;

            //Print(points, targetX, targetY);

            return totalRisk;
        }

        private void Print(Dictionary<string, Point> points, int maxx, int maxy)
        {
            var sb = new StringBuilder();

            for (var y = 0; y <= maxy; y++)
            {
                for (var x = 0; x <= maxx; x++)
                {
                    if (points.ContainsKey($"{x},{y}"))
                    {
                        var point = points[$"{x},{y}"];
                        if (point.ErosionLevel == 0)
                        {
                            sb.Append('?');
                        }
                        else
                        {
                            switch (point.Type)
                            {
                                case 0: sb.Append('.'); break;
                                case 1: sb.Append('='); break;
                                case 2: sb.Append('|'); break;
                                default: throw new Exception("Type is off");
                            }
                        }
                    }
                    else
                    {
                        sb.Append('x');
                    }
                }

                sb.AppendLine();
            }

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(sb.ToString());
            Console.ReadLine();
        }

        private class Point
        {
            public Point(int x, int y, int depth, ref Dictionary<string, Point> points, bool isTarget = false)
            {
                X = x;
                Y = y;
                Depth = depth;
                _points = points;
                Hash = $"{x},{y}";

                if (y == 0 && x == 0)
                {
                    GeologicIndex = 0;
                }
                else if (y == 0)
                {
                    GeologicIndex = x * 16807;
                }
                else if (x == 0)
                {
                    GeologicIndex = y * 48271;
                }
                else
                {
                    GeologicIndex = W.ErosionLevel * N.ErosionLevel;
                }

                if (isTarget)
                {
                    GeologicIndex = 0;
                }

                ErosionLevel = (GeologicIndex + Depth) % 20183;
                Type = ErosionLevel % 3;
            }

            public int X { get; }
            public int Y { get; }
            private Dictionary<string, Point> _points;
            public int Depth { get; set; }
            public string Hash { get; }

            public int GeologicIndex { get; }
            public int ErosionLevel { get; }
            public int Type { get; }

            public Point N => 
                _points
                .ContainsKey($"{X},{Y - 1}") 
                    ? _points[$"{X},{Y - 1}"] 
                    : _points[$"{X},{Y - 1}"] = new Point(X, Y - 1, Depth, ref _points);

            public Point W => 
                _points
                .ContainsKey($"{X - 1},{Y}") 
                    ? _points[$"{X - 1},{Y}"] 
                    : _points[$"{X - 1},{Y}"] = new Point(X - 1, Y, Depth, ref _points);
        }
    }
}