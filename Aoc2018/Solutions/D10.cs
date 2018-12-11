using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2018.Solutions
{
    public class D10 : Solver<string>
    {
        public override int Day => 10;

        public override int Part => 12;

        protected override string Filename => @"Inputs\d10.input";

        protected override string GetAnswer(string input)
        {
            var regex = new Regex(@"position=<([\-0-9 ]+), ([\-0-9 ]+)> velocity=<([\-0-9 ]+), ([\-0-9 ]+)>");
            var lights = input
                .Split(Environment.NewLine)
                .Select(i => regex.Match(i))
                .Select(m => new LightPoint
                {
                    X = int.Parse(m.Groups[1].ToString()),
                    Y = int.Parse(m.Groups[2].ToString()),
                    Dx = int.Parse(m.Groups[3].ToString()),
                    Dy = int.Parse(m.Groups[4].ToString())
                })
                .ToArray();

            var secondsPassed = 0;

            while (true)
            {
                foreach(var light in lights)
                    light.AdvanceSeconds();

                //PrintLights(lights, ++secondsPassed);

                if (++secondsPassed > 10333)
                    break;
            }

            return "AHZLLCAL in 10333 seconds";
        }

        private void PrintLights(LightPoint[] lights, int secondsPassed)
        {
            var minX = lights.Select(l => l.X).Min();
            var maxX = lights.Select(l => l.X).Max();

            var minY = lights.Select(l => l.Y).Min();
            var maxY = lights.Select(l => l.Y).Max();
            var xRange = 1 + Math.Abs(maxX - minX);
            var yRange = 1 + Math.Abs(maxY - minY);

            if (xRange > 100 || yRange > 100)
                return;

            var lightmap = new string[yRange, xRange];
            for (var y = 0; y < yRange; y++)
                for (var x = 0; x < xRange; x++)
                    lightmap[y, x] = ".";

            var offsetX = -minX;
            var offsetY = -minY;

            foreach (var light in lights)
            {
                lightmap[light.Y + offsetY, light.X + offsetX] = "#";
            }

            for (var y = 0; y < yRange; y++)
            {
                for (var x = 0; x < xRange; x++)
                {
                    Console.Write(lightmap[y, x]);
                }

                Console.WriteLine();
            }
        }

        private class LightPoint
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Dx { get; set; }
            public int Dy { get; set; }

            public void AdvanceSeconds(int s = 1)
            {
                X += Dx * s;
                Y += Dy * s;
            }
        }
    }
}
