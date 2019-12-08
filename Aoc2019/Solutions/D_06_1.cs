using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2019.Solutions
{
    public class D_06_1 : Solver<int>
    {
        public override int Day => 6;

        public override int Part => 1;

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

            var com = spaceObjects["COM"];

            return CountOrbits(com, 0);
        }

        private int CountOrbits(SpaceObject current, int depth)
        {
            var orbitCount = 0;

            orbitCount += current.Orbiters.Count;

            foreach (var orbiter in current.Orbiters)
            {
                var subCount = CountOrbits(orbiter, depth + 1);
                orbitCount += subCount + depth;
            }

            return orbitCount;
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
    }
}
