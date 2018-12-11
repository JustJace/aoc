using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2018.Solutions
{
    public class D03P2 : Solver<int>
    {
        public override int Day => 3;
        public override int Part => 2;
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

            for (var i = 0; i < claims.Length; i++)
            {
                var ci = claims[i];
                var noOverlap = true;
                for (var j = 0; j < claims.Length; j++)
                {
                    if (i == j) continue;

                    var cj = claims[j];

                    if (ClaimsOverlap(ci, cj))
                    {
                        noOverlap = false;
                        break;
                    }
                }

                if (noOverlap)
                {
                    return ci.ClaimId;
                }
            }
            
            return 0;
        }

        private bool ClaimsOverlap(Claim c1, Claim c2)
        {
            bool xOverlap = ValueInRange(c1.X, c2.X, c2.X + c2.Width) 
                          ||ValueInRange(c2.X, c1.X, c1.X + c1.Width);

            bool yOverlap = ValueInRange(c1.Y, c2.Y, c2.Y + c2.Height)
                          ||ValueInRange(c2.Y, c1.Y, c1.Y + c1.Height);

            return xOverlap && yOverlap;
        }

        private bool ValueInRange(int value, int min, int max)
        {
            return (value >= min) && (value <= max);
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
