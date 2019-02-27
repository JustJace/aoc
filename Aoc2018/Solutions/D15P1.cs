using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Aoc2018.Solutions
{
    public class D15P1 : Solver<int>
    {
        public override int Day => 15;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d15.input";

        protected override int GetAnswer(string input)
        {
            var (map, units) = ParseInput(input);

            var roundsComplete = 0;

            while (true)
            {
                foreach (var unit in UnitsInCombatOrder(units))
                {
                    if (unit.IsAlive)
                    {
                        Move(map, units, unit);
                        Attack(map, units, unit);
                    }
                }

                RemoveDeadUnits(units);

                //Print(map, units, roundsComplete);

                if (OnlyOneUnitTypeLeft(units))
                    break;

                roundsComplete++;
            }

            return roundsComplete * units.Values.Select(u => u.HP).Sum();
        }

        private void Print(char[][] map, Dictionary<string, Unit> units, int rounds)
        {
            var sb = new StringBuilder();

            for (var y = 0; y < map.Length; y++)
            {
                var unitHps = "";
                for (var x = 0; x < map[y].Length; x++)
                {
                    if (units.ContainsKey($"{x},{y}"))
                    {
                        var unit = units[$"{x},{y}"];
                        var unitChar = unit.UnitType == UnitType.Elf ? "E" : "G";
                        sb.Append(unitChar);
                        unitHps += $" {unitChar}({unit.HP}) ";
                    }
                    else
                        sb.Append(map[y][x]);
                }

                sb.Append($" {unitHps}");
                sb.AppendLine();
            }
            sb.AppendLine($"Round: {rounds}");
            Console.Clear();
            Console.WriteLine(sb.ToString());
            Console.In.ReadLine();
        }

        private IEnumerable<Unit> UnitsInCombatOrder(Dictionary<string, Unit> units) 
            => units.Values
            .OrderBy(u => u.Location.Y)
            .ThenBy(u => u.Location.X);
        private bool OnlyOneUnitTypeLeft(Dictionary<string, Unit> units)
            => 1 == units.Values
            .Select(u => u.UnitType)
            .Distinct()
            .Count();

        private void RemoveDeadUnits(Dictionary<string, Unit> units)
        {
            var deadUnits = new List<string>();
            foreach (var kvp in units)
                if (!kvp.Value.IsAlive)
                    deadUnits.Add(kvp.Key);

            foreach (var deadUnit in deadUnits)
                units.Remove(deadUnit);
        }

        private void Move(char[][] map, Dictionary<string, Unit> units, Unit unit)
        {
            if (IsEnemy(units, unit, unit.Location.Up)) return;
            if (IsEnemy(units, unit, unit.Location.Left)) return;
            if (IsEnemy(units, unit, unit.Location.Right)) return;
            if (IsEnemy(units, unit, unit.Location.Down)) return;

            var moveTo = MoveTo(map, units, unit);
            if (moveTo == null) return;

            units.Remove(unit.Location.Hash);

            unit.Location = moveTo;

            units[unit.Location.Hash] = unit;
        }

        private bool IsEmpty(char[][] map, Dictionary<string, Unit> units, Point p) 
            => map[p.Y][p.X] == '.' 
            && (!units.ContainsKey(p.Hash)
              ||!units[p.Hash].IsAlive);

        private bool IsEnemy(Dictionary<string, Unit> units, Unit unit, Point p) 
            => (units.ContainsKey(p.Hash) 
            && units[p.Hash].IsAlive
            && units[p.Hash].UnitType == unit.EnemyType);

        private Point MoveTo(char[][] map, Dictionary<string, Unit> units, Unit unit)
        {
            var enemies = units.Values.Where(u => u.UnitType == unit.EnemyType && u.IsAlive);
            var enemyAdjacentPointsFree = new Dictionary<string, Point>();

            foreach (var enemy in enemies)
            {
                if (IsEmpty(map, units, enemy.Location.Up))
                    enemyAdjacentPointsFree[enemy.Location.Up.Hash] = enemy.Location.Up;

                if (IsEmpty(map, units, enemy.Location.Left))
                    enemyAdjacentPointsFree[enemy.Location.Left.Hash] = enemy.Location.Left;

                if (IsEmpty(map, units, enemy.Location.Right))
                    enemyAdjacentPointsFree[enemy.Location.Right.Hash] = enemy.Location.Right;

                if (IsEmpty(map, units, enemy.Location.Down))
                    enemyAdjacentPointsFree[enemy.Location.Down.Hash] = enemy.Location.Down;
            }

            var pointQueue = new Queue<SearchNode>();
            pointQueue.Enqueue(new SearchNode(unit.Location, null));

            var seenHash = new HashSet<string>
            {
                unit.Location.Hash
            };

            while (pointQueue.Any())
            {
                var currentNode = pointQueue.Dequeue();

                if (enemyAdjacentPointsFree.ContainsKey(currentNode.Location.Hash))
                    return currentNode.GetFirstMove();

                if (!seenHash.Contains(currentNode.Location.Up.Hash) 
                && IsEmpty(map, units, currentNode.Location.Up))
                {
                    pointQueue.Enqueue(new SearchNode(currentNode.Location.Up, currentNode));
                    seenHash.Add(currentNode.Location.Up.Hash);
                }

                if (!seenHash.Contains(currentNode.Location.Left.Hash) 
                && IsEmpty(map, units, currentNode.Location.Left))
                {
                    pointQueue.Enqueue(new SearchNode(currentNode.Location.Left, currentNode));
                    seenHash.Add(currentNode.Location.Left.Hash);
                }

                if (!seenHash.Contains(currentNode.Location.Right.Hash)
                && IsEmpty(map, units, currentNode.Location.Right))
                {
                    pointQueue.Enqueue(new SearchNode(currentNode.Location.Right, currentNode));
                    seenHash.Add(currentNode.Location.Right.Hash);
                }

                if (!seenHash.Contains(currentNode.Location.Down.Hash)
                && IsEmpty(map, units, currentNode.Location.Down))
                {
                    pointQueue.Enqueue(new SearchNode(currentNode.Location.Down, currentNode));
                    seenHash.Add(currentNode.Location.Down.Hash);
                }
            }

            return null;
        }

        private class SearchNode
        {
            public SearchNode(Point p, SearchNode previous)
            {
                Location = p;
                PreviousNode = previous;
            }
            public Point Location { get; set; }
            public SearchNode PreviousNode { get; set; }

            public Point GetFirstMove()
            {
                var current = this;
                while (current.PreviousNode.PreviousNode != null)
                    current = current.PreviousNode;
                return current.Location;
            }
        }

        private void Attack(char[][] map, Dictionary<string, Unit> units, Unit unit)
        {
            var enemiesInRange = new List<Unit>();

            if (IsEnemy(units, unit, unit.Location.Up))
                enemiesInRange.Add(units[unit.Location.Up.Hash]);

            if (IsEnemy(units, unit, unit.Location.Left))
                enemiesInRange.Add(units[unit.Location.Left.Hash]);

            if (IsEnemy(units, unit, unit.Location.Right))
                enemiesInRange.Add(units[unit.Location.Right.Hash]);

            if (IsEnemy(units, unit, unit.Location.Down))
                enemiesInRange.Add(units[unit.Location.Down.Hash]);

            if (!enemiesInRange.Any())
                return;

            var attackEnemy = enemiesInRange
                .OrderBy(e => e.HP)
                .ThenBy(e => e.Location.Y)
                .ThenBy(e => e.Location.X)
                .First();

            attackEnemy.HP -= unit.AP;
        }

        private Tuple<char[][], Dictionary<string, Unit>> ParseInput(string input)
        {
            var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
            var units = new Dictionary<string, Unit>();

            for (var y = 0; y < map.Length; y++)
                for (var x = 0; x < map[y].Length; x++)
                    if (Unit.CharIsUnit(map[y][x]))
                    {
                        units[$"{x},{y}"] = new Unit(x, y, map[y][x]);
                        map[y][x] = '.';
                    }

            return new Tuple<char[][], Dictionary<string, Unit>>(map, units);
        }

        private enum UnitType { Elf, Goblin };

        private class Unit
        {
            public Unit(int x, int y, char unitChar)
            {
                Location = new Point(x, y);
                UnitType = unitChar == 'G' ? UnitType.Goblin : UnitType.Elf;
                EnemyType = unitChar == 'G' ? UnitType.Elf : UnitType.Goblin;
                AP = 3;
                HP = 200;
            }

            public Point Location { get; set; }
            public UnitType UnitType { get; }
            public UnitType EnemyType { get; }
            public int AP { get; }
            public int HP { get; set; }
            public bool IsAlive => HP > 0;
            public static bool CharIsUnit(char c)
            {
                return c == 'G' || c == 'E';
            }
        }

        private class Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
                Hash = $"{X},{Y}";
            }
            public int X { get; set; }
            public int Y { get; set; }
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
