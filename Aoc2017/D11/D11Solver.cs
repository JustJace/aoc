using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Aoc2017.D11
{
    
    public class D11Solver
    {
        enum HexDirection { N, S, NW, NE, SW, SE }

        public int SolveP1() => EstimateP1(File.ReadAllText(@"D11\D11.input"));
        internal int EstimateP1(string input)
        {
            var childPath = ParseInput(input);
            var n = childPath.Count(c => c == HexDirection.N);
            var nw = childPath.Count(c => c == HexDirection.NW);
            var ne = childPath.Count(c => c == HexDirection.NE);
            var s = childPath.Count(c => c == HexDirection.S);
            var sw = childPath.Count(c => c == HexDirection.SW);
            var se = childPath.Count(c => c == HexDirection.SE);

            var northDelta = n - s;
            var northEastDelta = ne - sw;
            var northWestDelta = nw - se;

            // https://www.redblobgames.com/grids/hexagons/#distances
            int x = northDelta + northWestDelta;
            int y = - northDelta - northEastDelta;
            int z = northEastDelta - northWestDelta;
            return HexDistance(x, y, z);
        }

        private List<HexDirection> ParseInput(string input)
        {
            return input.Split(',')
                        .Select(s => (HexDirection)Enum.Parse(typeof(HexDirection), s.ToUpper()))
                        .ToList();
        }

        private int HexDistance(int x, int y, int z) => (Math.Abs(x) + Math.Abs(y) + Math.Abs(z)) / 2;

        public int SolveP2()
        {
            var childPath = ParseInput(File.ReadAllText(@"D11\D11.input"));
            int x = 0, y = 0, z = 0;
            var bestMax = 0;

            Action n = () => { x++; y--; };
            Action s = () => { x--; y++; };

            Action ne = () => { z++; y--; };
            Action sw = () => { z--; y++; };

            Action nw = () => { x++; z--; };
            Action se = () => { x--; z++; };

            foreach (var childStep in childPath)
            {
                switch (childStep)
                {
                    case HexDirection.N: n(); break;
                    case HexDirection.S: s(); break;
                    case HexDirection.NE: ne(); break;
                    case HexDirection.NW: nw(); break;
                    case HexDirection.SE: se(); break;
                    case HexDirection.SW: sw(); break;

                    default: throw new Exception("you screwed up.");
                }

                var distance = HexDistance(x, y, z);
                if (distance > bestMax)
                    bestMax = distance;
            }

            return bestMax;
        }
    }
}
