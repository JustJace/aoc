using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2018.Solutions
{
    public class D03P1 : Solver<int>
    {
        public override int Day => 3;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d03.input";

        protected override int GetAnswer(string input)
        {
            var regex = new Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)");
            var claims = input
                .Split(Environment.NewLine)
                .Select(i => regex.Match(i))
                .Select(m => new Claim
                {
                    ClaimId = int.Parse(m.Groups[1].ToString()),
                    X = int.Parse(m.Groups[2].ToString()),
                    Y = int.Parse(m.Groups[3].ToString()),
                    Width = int.Parse(m.Groups[4].ToString()),
                    Height = int.Parse(m.Groups[5].ToString())
                })
                .ToArray();

            var claimed = new int[1000, 1000];

            foreach (var claim in claims)
                for (var x = claim.X; x < claim.X + claim.Width; x++)
                    for (var y = claim.Y; y < claim.Y + claim.Height; y++)
                        claimed[x, y]++;

            var overlapCount = 0;

            for (var x = 0; x < 1000; x++)
                for (var y = 0; y < 1000; y++)
                    if (claimed[x, y] > 1)
                        overlapCount++;

            return overlapCount;
        }

        private class Claim
        {
            public int ClaimId { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}
