using System;
using System.Collections.Generic;
using System.Text;

namespace Aoc2018.Solutions
{
    public class D2P2 : Solver<string>
    {
        public override int Day => 2;
        public override int Part => 2;
        protected override string Filename => @"Inputs\d2p1.input";

        // Implementation is awful, but quickly gets right answer with low number of inputs
        protected override string GetAnswer(string input)
        {
            var ids = input.Split(Environment.NewLine);

            for (var i = 0; i < ids.Length; i++)
            {
                for (var j = i + 1; j< ids.Length; j++)
                {
                    var idi = ids[i];
                    var idj = ids[j];
                    var charsOff = 0;

                    for (var k = 0; k < idi.Length; k++)
                    {
                        if (idi[k] != idj[k])
                            charsOff++;
                    }

                    if (charsOff == 1)
                    {
                        var answer = "";
                        for (var k = 0; k < idi.Length; k++)
                        {
                            if (idi[k] == idj[k])
                                answer += idi[k];
                        }
                        return answer;
                    }
                }
            }

            return "";
        }
    }
}
