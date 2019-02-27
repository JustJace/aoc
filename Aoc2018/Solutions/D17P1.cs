using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Aoc2018.Solutions
{
    public class D17P1 : Solver<int>
    {
        public override int Day => 17;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d17.input";

        protected override int GetAnswer(string input)
        {
            var veins = ParseInput(input);
            var xmax = veins.Select(v => v.Xmax).Max();
            var ymin = veins.Select(v => v.Ymin).Min();
            var ymax = veins.Select(v => v.Ymax).Max();
            var clay = new HashSet<string>();
            var springx = 500;
            var springy = 0;

            foreach (var vein in veins)
            {
                for (var y = vein.Ymin; y <= vein.Ymax; y++)
                    for (var x = vein.Xmin; x <= vein.Xmax; x++)
                        clay.Add($"{x},{y}");
            }

            return DFS(clay, new Node(new Point(springx, springy), null), ymax, ymin, new Dictionary<string, Node>());
        }

        private void MarkFlowing(Node n, StackSet<Node> stack, HashSet<string> clay, Dictionary<string, Node> seen)
        {
            n.Flowing = true;
            PrintArea(stack, clay, seen, n, true);
            if (n.Left != null && !n.Left.Flowing && n.Left.Visited)
                MarkFlowing(n.Left, stack, clay, seen);

            if (n.Right != null && !n.Right.Flowing && n.Right.Visited)
                MarkFlowing(n.Right, stack, clay, seen);

            if (n.Previous != null && !n.Previous.Flowing)
                MarkFlowing(n.Previous, stack, clay, seen);
        }

        private int DFS(HashSet<string> clay, Node spring, int ymax, int ymin, Dictionary<string, Node> seen)
        {
            var stack = new StackSet<Node>();
            stack.Push(spring);

            var answer = 0;

            while (stack.Any())
            {
                var current = stack.Pop();
                seen[current.Location.Hash] = current;

                if (current.Location.Y <= ymax && current.Location.Y >= ymin && !current.Visited)
                {
                    current.Visited = true;
                    answer++;
                }

                if (current.Location.Y > ymax)
                {
                    MarkFlowing(current, stack, clay, seen);
                    continue;
                }

                PrintArea(stack, clay, seen, current, false);

                // down
                if (!clay.Contains(current.Location.Down.Hash))
                {
                    if (!seen.ContainsKey(current.Location.Down.Hash))
                    {
                        var node = new Node(current.Location.Down, current);
                        stack.Push(node);
                        continue;
                    }
                    else if (seen[current.Location.Down.Hash].Flowing)
                    {
                        MarkFlowing(current, stack, clay, seen);
                        continue;
                    }
                }

                var leftAdded = false;
                var rightAdded = false;
                // check left
                if (!clay.Contains(current.Location.Left.Hash))
                {
                    if (!seen.ContainsKey(current.Location.Left.Hash))
                    {
                        var newLeft = new Node(current.Location.Left, current)
                        {
                            Right = current
                        };
                        current.Left = newLeft;
                        stack.Push(newLeft);
                        leftAdded = true;
                    }
                    else if (seen[current.Location.Left.Hash].Flowing)
                    {
                        current.Flowing = true;
                    }
                }

                // check Right
                if (!clay.Contains(current.Location.Right.Hash))
                {
                    if (!seen.ContainsKey(current.Location.Right.Hash)) 
                    {
                        var newRight = new Node(current.Location.Right, current)
                        {
                            Left = current
                        };
                        current.Right = newRight;
                        stack.Push(newRight);
                        rightAdded = true;
                    }
                    else if (seen[current.Location.Right.Hash].Flowing)
                    {
                        current.Flowing = true;
                    }
                }

                if (!leftAdded && !rightAdded && current.Previous != null)
                {
                    stack.Push(current.Previous);
                }
            }

            return answer;
        }

        private class StackSet<T>
            where T : Node
        {
            private Stack<T> _stack = new Stack<T>();
            private HashSet<string> _hash = new HashSet<string>();

            public void Push(T item)
            {
                if (!_hash.Contains(item.Location.Hash))
                {
                    _stack.Push(item);
                    _hash.Add(item.Location.Hash);
                }
            }

            public T Pop()
            {
                var item = _stack.Pop();
                _hash.Remove(item.Location.Hash);
                return item;
            }

            public bool Contains(T item)
            {
                return Contains(item.Location.Hash);
            }

            public bool Contains(string hash)
            {
                return _hash.Contains(hash);
            }

            public bool Any()
            {
                return _hash.Any();
            }
        }

        private void PrintArea(StackSet<Node> stack, HashSet<string> clay, Dictionary<string, Node> seen, Node current, bool flowing)
        {
            return;
            if (current == null) return;
            var sb = new StringBuilder();
            var yRange = 30;
            var xRange = 60;
            for (var y = current.Location.Y - yRange; y < current.Location.Y + yRange; y++)
            {
                for (var x = current.Location.X - xRange; x < current.Location.X + xRange; x++)
                {
                    if (stack.Contains($"{x},{y}"))
                        sb.Append('q');
                    else if (current.Location.X == x && current.Location.Y == y)
                        sb.Append('@');
                    else if (clay.Contains($"{x},{y}"))
                        sb.Append('#');
                    else if (seen.ContainsKey($"{x},{y}"))
                    {
                        if (seen[$"{x},{y}"].Flowing)
                            sb.Append('~');
                        else
                            sb.Append('|');
                    }
                    else
                        sb.Append('.');
                }

                sb.Append($"{y}".PadLeft(5));

                sb.AppendLine();
            }

            sb.AppendLine($"Flowing: {flowing}");

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(sb.ToString());
        }

        private class Node
        {
            public Node(Point location, Node previous)
            {
                Location = location;
                Previous = previous;
            }
            public Node(Point location, Node previous, bool flowing)
            {
                Location = location;
                Previous = previous;
                Flowing = flowing;
            }

            public Point Location { get; }
            public Node Previous { get; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public bool Flowing { get; set; }
            public bool Visited { get; set; }
        }

        private void PrintSoil(char[,] soil)
        {
            var sb = new StringBuilder();

            for (var y = 0; y < soil.GetLength(0); y++)
            {
                for (var x = 0; x < soil.GetLength(1); x++)
                {
                    sb.Append(soil[y, x]);
                }

                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
            Console.In.ReadLine();
        }

        private List<ClayVein> ParseInput(string input)
        {
            var veinRegex = new Regex(@"([x|y])=(\d+), [x|y]=(\d+)..(\d+)");

            return input
                .Split(Environment.NewLine)
                .Select(l => veinRegex.Match(l))
                .Select(m =>
                {
                    var var1 = m.Groups[1].ToString();
                    var var1minmax = int.Parse(m.Groups[2].ToString());
                    var var2min = int.Parse(m.Groups[3].ToString());
                    var var2max = int.Parse(m.Groups[4].ToString());

                    int xmin, xmax, ymin, ymax = 0;
                    if (var1 == "x")
                    {
                        xmin = xmax = var1minmax;
                        ymin = var2min;
                        ymax = var2max;
                    }
                    else
                    {
                        ymin = ymax = var1minmax;
                        xmin = var2min;
                        xmax = var2max;
                    }

                    return new ClayVein(xmin, xmax, ymin, ymax);
                })
                .ToList();
        }

        private class ClayVein
        {
            public ClayVein(int xmin, int xmax, int ymin, int ymax)
            {
                Xmin = xmin;
                Xmax = xmax;
                Ymin = ymin;
                Ymax = ymax;
            }

            public int Xmin { get; }
            public int Xmax { get; }
            public int Ymin { get; }
            public int Ymax { get; }
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
        }
    }
}
