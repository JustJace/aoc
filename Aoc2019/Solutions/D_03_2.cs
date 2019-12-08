using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2019.Solutions
{
    public class D_03_2 : Solver<int>
    {
        public override int Day => 3;

        public override int Part => 2;

        protected override string Filename => @"Inputs\D_03.input";

        protected override int GetAnswer(string input)
        {
            var wireStrings = input.Split(Environment.NewLine).ToArray();
            var wire1 = wireStrings[0].Split(',').ToArray();
            var wire2 = wireStrings[1].Split(',').ToArray();

            var wireLocations1 = new Dictionary<string, int>();
            var stepCount = 0;
            var currentX = 0;
            var currentY = 0;
            var deltaX = 0;
            var deltaY = 0;
            foreach (var instruction in wire1)
            {
                var direction = instruction[0];
                var length = int.Parse(instruction.Substring(1));

                switch (direction)
                {
                    case 'U':
                        deltaX = 0;
                        deltaY = 1;
                        break;
                    case 'D':
                        deltaX = 0;
                        deltaY = -1;
                        break;
                    case 'L':
                        deltaX = -1;
                        deltaY = 0;
                        break;
                    case 'R':
                        deltaX = 1;
                        deltaY = 0;
                        break;
                }

                for (var i = 0; i < length; i++)
                {
                    currentX += deltaX;
                    currentY += deltaY;
                    stepCount++;
                    var key = $"{currentX},{currentY}";
                    if (!wireLocations1.ContainsKey(key))
                    {
                        wireLocations1.Add(key, stepCount);
                    }
                }
            }

            var intersectionDelays = new List<int>();

            currentX = 0;
            currentY = 0;
            deltaX = 0;
            deltaY = 0;
            stepCount = 0;
            foreach (var instruction in wire2)
            {
                var direction = instruction[0];
                var length = int.Parse(instruction.Substring(1));

                switch (direction)
                {
                    case 'U':
                        deltaX = 0;
                        deltaY = 1;
                        break;
                    case 'D':
                        deltaX = 0;
                        deltaY = -1;
                        break;
                    case 'L':
                        deltaX = -1;
                        deltaY = 0;
                        break;
                    case 'R':
                        deltaX = 1;
                        deltaY = 0;
                        break;
                }

                for (var i = 0; i < length; i++)
                {
                    currentX += deltaX;
                    currentY += deltaY;
                    stepCount++;
                    var key = $"{currentX},{currentY}";

                    if (wireLocations1.ContainsKey(key))
                    {
                        intersectionDelays.Add(wireLocations1[key] + stepCount);
                    }
                }
            }

            return intersectionDelays.Min();
        }
    }
}
