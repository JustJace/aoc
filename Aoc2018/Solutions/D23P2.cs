using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Aoc2018.Solutions
{
    public class D23P2 : Solver<int>
    {
        public override int Day => 23;
        public override int Part => 2;
        protected override string Filename => @"Inputs\d23.input";

        protected override int GetAnswer(string input)
        {
            var botRegex = new Regex(@"pos=<([0-9-]+),([0-9-]+),([0-9-]+)>, r=(\d+)");
            var bots = input
                .Split(Environment.NewLine)
                .Select(l => botRegex.Match(l))
                .Select(m => new Bot
                (
                    int.Parse(m.Groups[1].ToString()),
                    int.Parse(m.Groups[2].ToString()),
                    int.Parse(m.Groups[3].ToString()),
                    int.Parse(m.Groups[4].ToString())
                ))
                .ToArray();

            var zero = new Coordinate(0, 0, 0);
            var best = zero;
            best = PrioritySearch(bots, best, (int)Math.Pow(10, 7), 60);
            best = PrioritySearch(bots, best, (int)Math.Pow(10, 6), 60000);
            best = PrioritySearch(bots, best, (int)Math.Pow(10, 5), 60000);
            best = PrioritySearch(bots, best, (int)Math.Pow(10, 4), 60000);
            best = PrioritySearch(bots, best, (int)Math.Pow(10, 3), 60000);
            best = PrioritySearch(bots, best, (int)Math.Pow(10, 2), 60000);
            best = PrioritySearch(bots, best, (int)Math.Pow(10, 1), 60000);
            best = PrioritySearch(bots, best, (int)Math.Pow(10, 0), 60000);
            return ManhattenDistance3D(best, zero);
        }

        private Coordinate PrioritySearch(Bot[] bots, Coordinate start, int resolution, int searchSpace)
        {
            var avgX = (int)bots.Average(b => b.Coord.X);
            var avgY = (int)bots.Average(b => b.Coord.Y);
            var avgZ = (int)bots.Average(b => b.Coord.Z);
            var zero = new Coordinate(0, 0, 0);
            var queue = new FastPriorityQueue<Coordinate>(100000000);
            queue.Enqueue(start, 0);
            var seen = new HashSet<string>();
            seen.Add(start.Hash);

            var searched = 0;
            var best = 0;
            var bestManhatten = 0;
            var bestCoordinate = zero;

            while (queue.Any() && ++searched <= searchSpace)
            {
                var current = queue.Dequeue();
                var inRange = BotsInRangeOf(current, bots);
                if (inRange > best)
                {
                    best = inRange;
                    bestManhatten = ManhattenDistance3D(current, zero);
                    bestCoordinate = current;
                    Console.WriteLine($"Best in range: {best} at {bestCoordinate}. Distance to Zero: {bestManhatten}");
                }
                else if (inRange == best)
                {
                    var md = ManhattenDistance3D(current, zero);
                    if (md < bestManhatten)
                    {
                        bestManhatten = md;
                        bestCoordinate = current;
                        Console.WriteLine($"Best in range: {best} at {bestCoordinate}. Distance to Zero: {bestManhatten}");
                    }
                }

                CheckSeenToQueue(queue, seen, bots, current.X - resolution, current.Y, current.Z);
                CheckSeenToQueue(queue, seen, bots, current.X + resolution, current.Y, current.Z);
                CheckSeenToQueue(queue, seen, bots, current.X, current.Y - resolution, current.Z);
                CheckSeenToQueue(queue, seen, bots, current.X, current.Y + resolution, current.Z);
                CheckSeenToQueue(queue, seen, bots, current.X, current.Y, current.Z - resolution);
                CheckSeenToQueue(queue, seen, bots, current.X, current.Y, current.Z + resolution);
            }

            return bestCoordinate;
        }

        // 50426731 too low

        private void CheckSeenToQueue(FastPriorityQueue<Coordinate> queue, HashSet<string> seen, Bot[] bots, int x, int y, int z)
        {
            var c = new Coordinate(x, y, z);
            if (!seen.Contains(c.Hash))
            {
                queue.Enqueue(c, CalcPriority(bots, c));
                seen.Add(c.Hash);
            }
        }

        private float CalcPriority(Bot[] bots, Coordinate c)
        {
            // average distance to bot center

            var distances = new List<double>();

            foreach (var bot in bots)
            {
                var dist = ManhattenDistance3D(bot.Coord, c) - bot.R;
                if (dist < 0)
                    dist = 0;
                distances.Add(dist);
            }

            return (float)distances.Sum();
        }

        private int BotsInRangeOf(Coordinate c, Bot[] bots)
        {
            var inRange = 0;
            foreach (var bot in bots)
            {
                var manDist = ManhattenDistance3D(c, bot.Coord);
                if (manDist <= bot.R)
                    inRange++;
            }
            return inRange;
        }

        private int ManhattenDistance3D(Coordinate c1, Coordinate c2)
        {
            return Math.Abs(c1.X - c2.X) + Math.Abs(c1.Y - c2.Y) + Math.Abs(c1.Z - c2.Z);
        }

        private class Bot
        {
            public Bot(int x, int y, int z, int r)
            {
                Coord = new Coordinate(x, y, z);
                R = r;
            }

            public Coordinate Coord { get; }
            public int R { get; }
        }

        private class Coordinate : FastPriorityQueueNode
        {
            public Coordinate(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
                Hash = $"{x},{y},{z}";
            }
            public override string ToString()
            {
                return $"({Hash})";
            }
            public string Hash { get; }
            public int X { get; }
            public int Y { get; }
            public int Z { get; }
        }
    }
}