using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2017.D10
{
    public class D10Solver
    {
        public int SolveP1()
        {
            var lengths = new int[] { 129, 154, 49, 198, 200, 133, 97, 254, 41, 6, 2, 1, 255, 0, 191, 108 };
            var numbers = Enumerable.Range(0, 256).ToList();
            var skip = 0;
            var currentPosition = 0;

            foreach (var length in lengths)
            {
                if (length < numbers.Count && length > 1)
                {
                    var range = RangeCircular(numbers, currentPosition, length);
                    range.Reverse();
                    RangeReplaceCircular(numbers, range, currentPosition);
                }
              
                currentPosition += length + skip;
                skip++;
            }

            return numbers[0] * numbers[1];
        }

        private List<int> RangeCircular(List<int> numbers, int startPosition, int length)
        {
            var range = new List<int>();
            for (var i = startPosition; i < startPosition + length; i++)
            {
                range.Add(numbers[i % numbers.Count]);
            }
            return range;
        }

        private void RangeReplaceCircular(List<int> numbers, List<int> replace, int startPosition)
        {
            for (var i = 0; i < replace.Count; i++)
                numbers[(i + startPosition) % numbers.Count] = replace[i];
        }

        public string SolveP2()
        {
            var input = "129,154,49,198,200,133,97,254,41,6,2,1,255,0,191,108";
            var lengths = StringToAsciiCodes(input).Concat(new int[] { 17, 31, 73, 47, 23 });
            var numbers = Enumerable.Range(0, 256).ToList();
            var skip = 0;
            var currentPosition = 0;

            for (var i = 0; i < 64; i++)
            {
                foreach (var length in lengths)
                {
                    if (length < numbers.Count && length > 1)
                    {
                        var range = RangeCircular(numbers, currentPosition, length);
                        range.Reverse();
                        RangeReplaceCircular(numbers, range, currentPosition);
                    }

                    currentPosition += length + skip;
                    skip++;
                }
            }

            var denseHash = SparseToDenseHash(numbers);
            return HashToHex(denseHash).ToLower();
        }

        private List<int> SparseToDenseHash(List<int> sparseHash)
        {
            var denseHash = new List<int>();

            for (var offset = 0; offset < 16; offset++)
                denseHash.Add(XorNumbers(sparseHash.Skip(offset * 16).Take(16)));

            return denseHash;
        }

        private int XorNumbers(IEnumerable<int> numbers) => numbers.Aggregate((n1, n2) => n1 ^ n2);

        private string HashToHex(List<int> denseHash)
        {
            var hex = "";
            foreach (var val in denseHash)
                hex += val.ToString("X2");
            return hex;
        }

        private List<int> StringToAsciiCodes(string s) => s.ToCharArray().Select(c => (int)c).ToList();
    }
}
