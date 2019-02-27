using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Aoc2018.Solutions
{
    public class D20P1 : Solver<int>
    {
        public override int Day => 20;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d20.input";

        protected override int GetAnswer(string input)
        {
            //input = @"^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$";

            var directions = input.Substring(1, input.Length - 2);
            var start = new Room(0, 0, 0);
            var rooms = new Dictionary<string, Room>
            {
                [start.Location.Hash] = start
            };

            SearchRecursive(start, rooms, directions);

            //Print(rooms);

            return rooms.Values.Where(r => r.Depth > 1000).Count();
        }

        private void Print(Dictionary<string, Room> rooms)
        {
            var sb = new StringBuilder();

            var minx = rooms.Values.Select(v => v.Location.X).Min();
            var maxx = rooms.Values.Select(v => v.Location.X).Max();
            var miny = rooms.Values.Select(v => v.Location.Y).Min();
            var maxy = rooms.Values.Select(v => v.Location.Y).Max();

            for (var y = miny; y <= maxy; y++)
            {
                for (var x = minx; x <= maxx; x++)
                {
                    if (rooms.ContainsKey($"{x},{y}"))
                    {
                        //var depth = rooms[$"{x},{y}"].Depth;
                        //sb.Append($"{depth}".PadLeft(2).PadRight(3));
                        sb.Append(".");
                    }
                    else
                        sb.Append("#");
                }

                sb.Append($"{y}".PadLeft(5));
                sb.AppendLine();
            }

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(sb.ToString());
            Thread.Sleep(2);
            //Console.In.ReadLine();
        }

        private void SearchRecursive(Room startRoom, Dictionary<string,Room> rooms, string directions)
        {
            var currentRoom = startRoom;
            for (var i = 0; i < directions.Length; i++)
            {
                var direction = directions[i];
                if (IsDirection(direction))
                {
                    var pointInDirection = DirectionToPoint(currentRoom.Location, direction);
                    if (!rooms.ContainsKey(pointInDirection.Hash))
                    {
                        currentRoom = rooms[pointInDirection.Hash] = new Room(pointInDirection, currentRoom.Depth + 1);
                        Print(rooms);
                    }
                    else
                    {
                        return;
                    }
                }
                else if (direction == '(')
                {
                    var open = 1;
                    var startIndex = i + 1;

                    while (open > 0)
                    {
                        i++;
                        direction = directions[i];
                        if (direction == '(')
                            open++;
                        else if (direction == ')')
                            open--;
                    }

                    SearchRecursive(currentRoom, rooms, directions.Substring(startIndex, i - startIndex));
                }
                else if (direction == '|')
                {
                    SearchRecursive(startRoom, rooms, directions.Substring(i + 1));
                }
            }
        }

        private bool IsDirection(char c) => c == 'E' || c == 'N' || c == 'W' || c == 'S';
        private Point DirectionToPoint(Point p, char c)
        {
            switch (c)
            {
                case 'E': return p.E;
                case 'N': return p.N;
                case 'W': return p.W;
                case 'S': return p.S;
                default: throw new Exception("Didn't handle all point direction cases.");
            }
        }

        private class Room
        {
            public Room(int x, int y, int depth) : this(new Point(x, y), depth) { }
            public Room(Point p, int depth)
            {
                Location = p;
                Depth = depth;
            }
            public Point Location { get; set; }
            public int Depth { get; set; }
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
            private Point _n;
            public Point N => _n ?? (_n = new Point(X, Y - 1));
            private Point _w;
            public Point W => _w ?? (_w = new Point(X - 1, Y));
            private Point _e;
            public Point E => _e ?? (_e = new Point(X + 1, Y));
            private Point _s;
            public Point S => _s ?? (_s = new Point(X, Y + 1));
        }
    }
}
