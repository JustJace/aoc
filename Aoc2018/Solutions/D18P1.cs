using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Aoc2018.Solutions
{
    public class D18P2 : Solver<int>
    {
        public override int Day => 18;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d18.input";

        protected override int GetAnswer(string input)
        {
            var acres = ParseInput(input);

            var scoreHash = new Dictionary<int, List<int>>();

            var cycleStarts = 455;
            var cycleLength = 483 - 455;

            for (var m = 0; m < cycleStarts; m++)
                acres = MinutePass(acres);

            var minuteHash = new Dictionary<int, int>();
            for (var m = 0; m < cycleLength; m++)
            {
                acres = MinutePass(acres);
                minuteHash[m] = acres.Values.Count(a => a.Char == '|') * acres.Values.Count(a => a.Char == '#');
            }


            //var testAcres = ParseInput(input);
            //for (var m = 0; m < 1000000000; m++)
            //{
            //    testAcres = MinutePass(testAcres);
            //    if (m >= cycleStarts)
            //    {
            //        var testScore = testAcres.Values.Count(a => a.Char == '|') * testAcres.Values.Count(a => a.Char == '#');
            //        var computeScore = minuteHash[(m - cycleStarts) % cycleLength];
            //        Console.WriteLine($"Minute: {m},   Expected: {testScore},   Actual: {computeScore}");
            //    }
            //}

            return minuteHash[(1000000000 - cycleStarts - 1) % cycleLength];



            //for (var m = 0; m < 1000000000; m++)
            //{
            //    acres = MinutePass(acres);
            //    var newScore = acres.Values.Count(a => a.Char == '|') * acres.Values.Count(a => a.Char == '#');
            //    if (scoreHash.ContainsKey(newScore))
            //    {
            //        scoreHash[newScore].Add(m);
            //        var minutesSeen = scoreHash[newScore].Select(i => i.ToString()).Aggregate((i1,i2) => $"{i1}, {i2}");
            //        Console.WriteLine($"Re-saw {newScore} at minutes: {minutesSeen}");
            //    }
            //    else
            //    {
            //        scoreHash[newScore] = new List<int> { m };
            //    }
            //}

            return acres.Values.Count(a => a.Char == '|') * acres.Values.Count(a => a.Char == '#');
        }

        private Dictionary<string, Acre> MinutePass(Dictionary<string, Acre> acres)
        {
            var nextState = new Dictionary<string, Acre>();

            foreach (var kvp in acres)
            {
                var hash = kvp.Key;
                var acre = kvp.Value;
                var nextAcre = new Acre(acre.Location.X, acre.Location.Y, acre.Char);
                switch (acre.Char)
                {
                    case '.':
                        if (CountTypeAroundAcre(acres, acre, '|') >= 3)
                            nextAcre.Char = '|';
                        break;

                    case '|':
                        if (CountTypeAroundAcre(acres, acre, '#') >= 3)
                            nextAcre.Char = '#';
                        break;

                    case '#':
                        if (CountTypeAroundAcre(acres, acre, '#') < 1 || CountTypeAroundAcre(acres, acre, '|') < 1)
                            nextAcre.Char = '.';
                        break;
                }

                nextState[nextAcre.Location.Hash] = nextAcre;
            }

            return nextState;
        }

        private int CountTypeAroundAcre(Dictionary<string, Acre> acres, Acre current, char c)
        {
            var count = 0;

            if (acres.ContainsKey(current.Location.UpLeft.Hash) 
            && acres[current.Location.UpLeft.Hash].Char == c)
                count++;

            if (acres.ContainsKey(current.Location.Up.Hash)
            && acres[current.Location.Up.Hash].Char == c)
                count++;

            if (acres.ContainsKey(current.Location.UpRight.Hash)
            && acres[current.Location.UpRight.Hash].Char == c)
                count++;

            if (acres.ContainsKey(current.Location.Right.Hash)
            && acres[current.Location.Right.Hash].Char == c)
                count++;

            if (acres.ContainsKey(current.Location.DownRight.Hash)
            && acres[current.Location.DownRight.Hash].Char == c)
                count++;

            if (acres.ContainsKey(current.Location.Down.Hash)
            && acres[current.Location.Down.Hash].Char == c)
                count++;

            if (acres.ContainsKey(current.Location.DownLeft.Hash)
            && acres[current.Location.DownLeft.Hash].Char == c)
                count++;

            if (acres.ContainsKey(current.Location.Left.Hash)
            && acres[current.Location.Left.Hash].Char == c)
                count++;

            return count;
        }

        private bool IsAcreThisType(Acre acre, char c)
        {
            return acre.Char == c;
        }

        private Dictionary<string, Acre> ParseInput(string input)
        {
            var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
            var acres = new Dictionary<string, Acre>();
            for (var y = 0; y < map.Length; y++)
                for (var x = 0; x < map[y].Length; x++)
                {
                    var acre = new Acre(x, y, map[y][x]);
                    acres[acre.Location.Hash] = acre;
                }

            return acres;
        }

        private class Acre
        {
            public Acre(int x, int y, char c)
            {
                Location = new Point(x, y);
                Char = c;
            }
            public Point Location { get; set; }
            public char Char { get; set; }
        }

        private class Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
                Hash = $"{x},{y}";
            }

            public int X { get; }
            public int Y { get; }
            public string Hash { get; }

            private Point _up;
            public Point Up => _up ?? (_up = new Point(X, Y - 1));

            private Point _left;
            public Point Left => _left ?? (_left = new Point(X - 1, Y));

            private Point _right;
            public Point Right => _right ?? (_right = new Point(X + 1, Y));

            private Point _down;
            public Point Down => _down ?? (_down = new Point(X, Y + 1));

            private Point _upLeft;
            public Point UpLeft => _upLeft ?? (_upLeft = new Point(X - 1, Y - 1));

            private Point _upRight;
            public Point UpRight => _upRight ?? (_upRight = new Point(X + 1, Y - 1));

            private Point _downLeft;
            public Point DownLeft => _downLeft ?? (_downLeft = new Point(X - 1, Y + 1));

            private Point _downRight;
            public Point DownRight => _downRight ?? (_downRight = new Point(X + 1, Y + 1));
        }
    }
}
