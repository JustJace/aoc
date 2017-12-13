using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Aoc2017.D13
{
    public class D13Solver
    {
        class Scanner
        {
            public int Depth { get; set; }
            public int Range { get; set; }
            public int Period => Range * 2 - 2;
            public int Position { get; set; } = 0;
            public bool Reverse { get; set; } = false;
            public void Tick()
            {
                if (Position == Range - 1)
                    Reverse = true;
                else if (Position == 0)
                    Reverse = false;

                Position += Reverse ? -1 : 1;
            }
        }
        public int SolveP1() => Simulate(ParseInput());

        private Dictionary<int, Scanner> ParseInput()
        {
            var input = File.ReadAllText(@"D13\D13.input");
            var scannersData = input.Split(Environment.NewLine);
            var scanners = new Dictionary<int, Scanner>();

            foreach (var scannerDatum in scannersData)
            {
                var split = scannerDatum.Split(':').Select(s => s.Trim()).Select(int.Parse).ToArray();
                var depth = split[0];
                var range = split[1];
                scanners[depth] = new Scanner { Depth = depth, Range = range };
            }

            return scanners;
        }

        private int Simulate(Dictionary<int, Scanner> scanners)
        {
            var severity = 0;
            for (var packetPosition = 0; packetPosition <= scanners.Keys.Max(); packetPosition++)
            {
                if (scanners.ContainsKey(packetPosition))
                    if (scanners[packetPosition].Position == 0)
                        severity += scanners[packetPosition].Depth * scanners[packetPosition].Range;

                foreach (var scanner in scanners.Values)
                    scanner.Tick();
            }
            return severity;
        }

        public int SolveP2()
        {
            var scanners = ParseInput();

            var delays = Enumerable.Range(0, 10000000).ToList();

            foreach(var scanner in scanners.Values)
                delays.RemoveAll(delay => (delay + scanner.Depth) % scanner.Period == 0);

            return delays.Min();
        }
    }
}
