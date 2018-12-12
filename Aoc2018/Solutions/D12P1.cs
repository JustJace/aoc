using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2018.Solutions
{
    public class D12P1 : Solver<int>
    {
        public override int Day => 12;
        public override int Part => 1;

        protected override string Filename => @"Inputs\d12.input";

        protected override int GetAnswer(string input)
        {
            var initialRegex = new Regex(@"initial state: ([\.#]+)");
            var generationRegex = new Regex(@"([\.#]{5}) => ([\.#])");
            var inputs = input.Split(Environment.NewLine);

            var plantList = new PlantList
            {
                Plants = initialRegex.Match(inputs[0]).Groups[1].ToString()
            };

            var rules = inputs
                .Skip(2)
                .Select(i => generationRegex.Match(i))
                .Select(m => new GenerationRule
                {
                    Pattern = m.Groups[1].ToString(),
                    Result = m.Groups[2].ToString()
                })
                .ToArray();

            for (long i = 0; i < 20; i++)
                NextGeneration(plantList, rules);

            return ScorePlants(plantList);
        }

        private int ScorePlants(PlantList plantList)
        {
            var total = 0;
            for (var i = 0; i < plantList.Plants.Length; i++)
                if (plantList.Plants[i] == '#')
                    total += i - plantList.ZeroOffset;
            return total;
        }

        private void NextGeneration(PlantList plantList, GenerationRule[] rules)
        {
            plantList.PadLeftIfNecessary();
            plantList.PadRightIfNecessary();

            var nextGeneration = "..";

            for (var i = 2; i < plantList.Plants.Length - 2; i++)
            {
                var substring = plantList.Plants.Substring(i - 2, 5);

                foreach (var rule in rules)
                {
                    if (substring == rule.Pattern)
                    {
                        nextGeneration += rule.Result;
                        break;
                    }
                }
            }

            nextGeneration += "..";
            plantList.Plants = nextGeneration;
        }

        private class PlantList
        {
            public string Plants { get; set; }
            public int ZeroOffset { get; set; }
            public void PadLeftIfNecessary()
            {
                var index = Plants.IndexOf('#');
                for (var i = 0; i < 5 - index; i++)
                {
                    Plants = "." + Plants;
                    ZeroOffset++;
                }
            }

            public void PadRightIfNecessary()
            {
                var index = Plants.LastIndexOf('#');
                for (var i = 0; i < 5 - (Plants.Length - 1 - index); i++)
                    Plants = Plants + ".";
            }
        }

        private class GenerationRule
        {
            public string Pattern { get; set; }
            public string Result { get; set; }
        }
    }
}
