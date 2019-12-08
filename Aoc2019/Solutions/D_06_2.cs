using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2019.Solutions
{
    public class D_06_2 : Solver<int>
    {
        public override int Day => 6;

        public override int Part => 2;

        protected override string Filename => @"Inputs\D_06.input";

        protected override int GetAnswer(string input)
        {
            var orbits = input
                .Split(Environment.NewLine)
                .ToArray();

            var spaceObjects = new Dictionary<string, SpaceObject>();

            foreach (var orbit in orbits)
            {
                var split = orbit.Split(')');
                var orbiteeName = split[0];
                var orbiterName = split[1];

                if (!spaceObjects.ContainsKey(orbiteeName))
                    spaceObjects[orbiteeName] = new SpaceObject(orbiteeName);

                if (!spaceObjects.ContainsKey(orbiterName))
                    spaceObjects[orbiterName] = new SpaceObject(orbiterName);

                var orbitee = spaceObjects[orbiteeName];
                var orbiter = spaceObjects[orbiterName];

                orbitee.Orbiters.Add(orbiter);
                orbiter.Orbits = orbitee;
            }

            var com = spaceObjects["YOU"];

            return BFS(com);
        }

        private int BFS(SpaceObject you)
        {
            var root = new Node(you.Orbits, 0);
            var queue = new Queue<Node>();
            var seen = new HashSet<string>() { "YOU" };
            queue.Enqueue(root);

            while (queue.Any())
            {
                var current = queue.Dequeue();

                if (current.SpaceObject.Orbiters.Any(o => o.Name == "SAN"))
                {
                    return current.Depth;
                }

                if (current.SpaceObject.Orbits != null && !seen.Contains(current.SpaceObject.Orbits.Name))
                {
                    seen.Add(current.SpaceObject.Orbits.Name);
                    queue.Enqueue(new Node(current.SpaceObject.Orbits, current.Depth + 1));
                }

                foreach (var orbiter in current.SpaceObject.Orbiters)
                {
                    if (!seen.Contains(orbiter.Name))
                    {
                        seen.Add(orbiter.Name);
                        queue.Enqueue(new Node(orbiter, current.Depth + 1));
                    }
                }
            }

            throw new Exception("Should have found SAN(ta)");
        }

        private class SpaceObject
        {
            public SpaceObject(string name)
            {
                Name = name;
            }

            public string Name { get; set; }
            public List<SpaceObject> Orbiters { get; } = new List<SpaceObject>();
            public SpaceObject Orbits { get; set; }
        }

        private class Node
        {
            public Node(SpaceObject spaceObject, int depth)
            {
                SpaceObject = spaceObject;
                Depth = depth;
            }

            public SpaceObject SpaceObject { get; }
            public int Depth { get; }
        }
    }
}
