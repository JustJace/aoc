using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Aoc2017.D9
{
    public class D9Solver
    {
        public int SolveP1()
        {
            var datastream = File.ReadAllText(@"D9\D9.input");
            var groupDepth = 0;
            var score = 0;
            var inGarbage = false;

            for (var pointer = 0; pointer < datastream.Length; pointer++)
            {
                switch (datastream[pointer])
                {
                    case '{':
                        if (!inGarbage)
                            groupDepth++;
                        break;
                    case '}':
                        if (!inGarbage)
                        {
                            score += groupDepth;
                            groupDepth--;
                        }
                        break;
                    case '!':
                        pointer++;
                        break;
                    case '<':
                        inGarbage = true;
                        break;
                    case '>':
                        inGarbage = false;
                        break;
                    case ',':
                    default:
                        break;
                }
            }

            return score;
        }

        public int SolveP2()
        {
            var datastream = File.ReadAllText(@"D9\D9.input");
            var garbageCount = 0;
            var inGarbage = false;

            for (var pointer = 0; pointer < datastream.Length; pointer++)
            {
                switch (datastream[pointer])
                {
                    case '{':
                        if (inGarbage)
                            garbageCount++;
                        break;
                    case '}':
                        if (inGarbage)
                            garbageCount++;
                        break;
                    case '!':
                        pointer++;
                        break;
                    case '<':
                        if (inGarbage)
                            garbageCount++;
                        else
                            inGarbage = true;
                        break;
                    case '>':
                        inGarbage = false;
                        break;
                    case ',':
                    default:
                        if (inGarbage)
                            garbageCount++;
                        break;
                }
            }

            return garbageCount;
        }
    }
}
