using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Aoc2018.Solutions
{
    public class D22P2 : Solver<int>
    {
        public override int Day => 22;
        public override int Part => 2;
        protected override string Filename => @"Inputs\d22.input";

        protected override int GetAnswer(string input)
        {
            var lines = input.Split(Environment.NewLine);
            var depth = int.Parse(lines[0].Split(' ')[1]);
            var targetXY = lines[1].Split(' ')[1].Split(',').Select(int.Parse).ToArray();
            var targetX = targetXY[0];
            var targetY = targetXY[1];

            var map = new Dictionary<string, Point>();
            map[$"0,0"] = new Point(0, 0, depth, ref map);

            return Search(ref map, targetX, targetY);
        }

        private int Search(ref Dictionary<string, Point> map, int targetX, int targetY)
        {
            var startState = new State(map[$"0,0"], Equipment.Torch, 0);
            var searchQueue = new FastPriorityQueue<State>(targetX * 3 * targetY * 3);
            var foundTimes = new List<int>();
            searchQueue.Enqueue(startState, 0);

            var seenStates = new Dictionary<string, State>()
            {
                [startState.Hash] = startState
            };

            while (searchQueue.Any())
            {
                var currentState = searchQueue.Dequeue();
                if (IsTargetState(currentState, targetX, targetY))
                {
                    //Console.WriteLine($"Found in {currentState.TotalMinutes} minutes");
                    foundTimes.Add(currentState.TotalMinutes);
                    continue;
                }

                //Print(map, currentState.Location.X, currentState.Location.Y, currentState.Wielding);

                if (currentState.Location.Y - 1 >= 0)
                {
                    var north = new State(currentState.Location.N, currentState.Wielding, currentState.TotalMinutes + 1);
                    if (ToolIsOk(currentState.Wielding, north.Location.Type))
                    {
                        if (!seenStates.ContainsKey(north.Hash) || seenStates[north.Hash].TotalMinutes > north.TotalMinutes)
                        {
                            searchQueue.Enqueue(north, north.TotalMinutes);
                            seenStates[north.Hash] = north;
                        }
                    }
                }

                if (currentState.Location.X - 1 >= 0)
                {
                    var west = new State(currentState.Location.W, currentState.Wielding, currentState.TotalMinutes + 1);
                    if (ToolIsOk(currentState.Wielding, west.Location.Type))
                    {
                        if (!seenStates.ContainsKey(west.Hash) || seenStates[west.Hash].TotalMinutes > west.TotalMinutes)
                        {
                            searchQueue.Enqueue(west, west.TotalMinutes);
                            seenStates[west.Hash] = west;
                        }
                    }
                }

                if (currentState.Location.Y <= targetY * 3)
                {
                    var south = new State(currentState.Location.S, currentState.Wielding, currentState.TotalMinutes + 1);
                    if (ToolIsOk(currentState.Wielding, south.Location.Type))
                    {
                        if (!seenStates.ContainsKey(south.Hash) || seenStates[south.Hash].TotalMinutes > south.TotalMinutes)
                        {
                            searchQueue.Enqueue(south, south.TotalMinutes);
                            seenStates[south.Hash] = south;
                        }
                    }
                }

                if (currentState.Location.X <= targetX * 3)
                {
                    var east = new State(currentState.Location.E, currentState.Wielding, currentState.TotalMinutes + 1);
                    if (ToolIsOk(currentState.Wielding, east.Location.Type))
                    {
                        if (!seenStates.ContainsKey(east.Hash) || seenStates[east.Hash].TotalMinutes > east.TotalMinutes)
                        {
                            searchQueue.Enqueue(east, east.TotalMinutes);
                            seenStates[east.Hash] = east;
                        }
                    }
                }
                
                if (ToolIsOk(Equipment.Gear, currentState.Location.Type))
                {
                    var wieldGear = new State(currentState.Location, Equipment.Gear, currentState.TotalMinutes + 7);
                    if (!seenStates.ContainsKey(wieldGear.Hash) || seenStates[wieldGear.Hash].TotalMinutes > wieldGear.TotalMinutes)
                    {
                        searchQueue.Enqueue(wieldGear, wieldGear.TotalMinutes);
                        seenStates[wieldGear.Hash] = wieldGear;
                    }
                }

                if (ToolIsOk(Equipment.Neither, currentState.Location.Type))
                {
                    var wieldNeither = new State(currentState.Location, Equipment.Neither, currentState.TotalMinutes + 7);
                    if (!seenStates.ContainsKey(wieldNeither.Hash) || seenStates[wieldNeither.Hash].TotalMinutes > wieldNeither.TotalMinutes)
                    {
                        searchQueue.Enqueue(wieldNeither, wieldNeither.TotalMinutes);
                        seenStates[wieldNeither.Hash] = wieldNeither;
                    }
                }

                if (ToolIsOk(Equipment.Torch, currentState.Location.Type))
                {
                    var wieldTorch = new State(currentState.Location, Equipment.Torch, currentState.TotalMinutes + 7);
                    if (!seenStates.ContainsKey(wieldTorch.Hash) || seenStates[wieldTorch.Hash].TotalMinutes > wieldTorch.TotalMinutes)
                    {
                        searchQueue.Enqueue(wieldTorch, wieldTorch.TotalMinutes);
                        seenStates[wieldTorch.Hash] = wieldTorch;
                    }
                }
            }

            return foundTimes.Min();
        }

        private readonly Dictionary<MapType, Dictionary<Equipment, bool>> _toolMap = new Dictionary<MapType, Dictionary<Equipment, bool>>
        {
            [MapType.Rock] = new Dictionary<Equipment, bool>
            {
                [Equipment.Gear] = true,
                [Equipment.Torch] = true,
                [Equipment.Neither] = false
            },
            [MapType.Wet] = new Dictionary<Equipment, bool>
            {
                [Equipment.Gear] = true,
                [Equipment.Torch] = false,
                [Equipment.Neither] = true
            },
            [MapType.Narrow] = new Dictionary<Equipment, bool>
            {
                [Equipment.Gear] = false,
                [Equipment.Torch] = true,
                [Equipment.Neither] = true
            },
        };
        private bool ToolIsOk(Equipment tool, MapType type)
        {
            return _toolMap[type][tool];
        }

        private bool IsTargetState(State state, int targetX, int targetY)
        {
            return state.Location.X == targetX && state.Location.Y == targetY && state.Wielding == Equipment.Torch;
        }

        private class State : FastPriorityQueueNode
        {
            public State(Point location, Equipment weilding, int totalMinutes)
            {
                Location = location;
                Wielding = weilding;
                TotalMinutes = totalMinutes;
                Hash = $"{Location.Hash},{(int)Wielding}";
            }

            public int TotalMinutes { get; set; }
            public Point Location { get; }
            public Equipment Wielding { get; }
            public string Hash { get; }
        }

        public enum Equipment: int { Neither = 0, Torch = 1, Gear = 2 }
        
        private void Print(Dictionary<string, Point> points, int currentX, int currentY, Equipment tool)
        {
            var sb = new StringBuilder();

            for (var y = currentY - 20; y <= currentY + 20; y++)
            {
                for (var x = currentX - 50; x <= currentX + 50; x++)
                {
                    if (x == currentX && y == currentY)
                    {
                        switch (tool)
                        {
                            case Equipment.Neither: sb.Append("N"); break;
                            case Equipment.Torch: sb.Append("T"); break;
                            case Equipment.Gear: sb.Append("G"); break;
                        }
                    }
                    else if (points.ContainsKey($"{x},{y}"))
                    {
                        var point = points[$"{x},{y}"];
                        switch (point.Type)
                        {
                            case MapType.Rock: sb.Append('.'); break;
                            case MapType.Wet: sb.Append('='); break;
                            case MapType.Narrow: sb.Append('|'); break;
                        }
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                }

                sb.AppendLine();
            }

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(sb.ToString());
            //Console.ReadLine();
        }

        private enum MapType : int { Rock = 0, Wet = 1, Narrow = 2};

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
                Type = (MapType)(ErosionLevel % 3);
            }

            public int X { get; }
            public int Y { get; }
            private Dictionary<string, Point> _points;
            public int Depth { get; set; }
            public string Hash { get; }

            public int GeologicIndex { get; }
            public int ErosionLevel { get; }
            public MapType Type { get; }

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

            public Point S => 
                _points
                .ContainsKey($"{X},{Y + 1}") 
                   ? _points[$"{X},{Y + 1}"] 
                   : _points[$"{X},{Y + 1}"] = new Point(X, Y + 1, Depth, ref _points);

            public Point E => 
                _points
                .ContainsKey($"{X + 1},{Y}") 
                   ? _points[$"{X + 1},{Y}"] 
                   : _points[$"{X + 1},{Y}"] = new Point(X + 1, Y, Depth, ref _points);
        }
    }
}