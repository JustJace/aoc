using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Aoc2018.Solutions
{
    public class D02P2 : Solver<string>
    {
        public override int Day => 2;
        public override int Part => 2;
        protected override string Filename => @"Inputs\d02.input";

        // Implementation is awful, but quickly gets right answer with low number of inputs
        protected override string GetAnswer(string input)
        {
            var ids = input.Split(Environment.NewLine);

            for (var i = 0; i + 1 < ids.Length; i++)
            {
                for (var j = i + 1; j < ids.Length; j++)
                {
                    var idi = ids[i];
                    var idj = ids[j];

                    var charsOff = 0;
                    var index = 0;

                    for (var k = 0; k < idi.Length; k++)
                    {
                        if (idi[k] != idj[k])
                        {
                            index = k;
                            if (++charsOff > 1) break;
                        }
                    }

                    if (charsOff == 1)
                    {
                        return idi.Remove(index, 1);
                    }
                }
            }

            return "";
        }
    }
}
