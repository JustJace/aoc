using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Aoc2018.Solutions
{
    public class D23P1 : Solver<int>
    {
        public override int Day => 23;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d23.input";

        protected override int GetAnswer(string input)
        {
//            input = @"pos=<0,0,0>, r=4
//pos=<1,0,0>, r=1
//pos=<4,0,0>, r=3
//pos=<0,2,0>, r=1
//pos=<0,5,0>, r=3
//pos=<0,0,3>, r=1
//pos=<1,1,1>, r=1
//pos=<1,1,2>, r=1
//pos=<1,3,1>, r=1";

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

            var strongestBot = bots.OrderByDescending(b => b.R).First();

            var inRange = 0;
            foreach (var bot in bots)
            {
                var manDist = ManhattenDistance3D(strongestBot, bot);
                if (manDist <= strongestBot.R)
                    inRange++;
            }

            return inRange;
        }

        private int ManhattenDistance3D(Bot b1, Bot b2)
        {
            return Math.Abs(b1.X - b2.X) + Math.Abs(b1.Y - b2.Y) + Math.Abs(b1.Z - b2.Z);
        }

        private class Bot
        {
            public Bot(int x, int y, int z, int r)
            {
                X = x;
                Y = y;
                Z = z;
                R = r;
            }

            public int X { get; }
            public int Y { get; }
            public int Z { get; }
            public int R { get; }
        }
    }
}