using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2018.Solutions
{
    public class D09P1 : Solver<int>
    {
        public override int Day => 9;

        public override int Part => 1;

        protected override string Filename => @"Inputs\d09.input";

        protected override int GetAnswer(string input)
        {
            var regex = new Regex(@"(\d+) players; last marble is worth (\d+) points");
            var match = regex.Match(input);
            var players = int.Parse(match.Groups[1].ToString());
            var marbles = int.Parse(match.Groups[2].ToString());

            var playedMarbles = new List<int>() { 0 };
            var playerScores = new int[players];

            var currentPlayer = 1;
            var currentMarble = 0;

            for (var m = 1; m <= marbles; m++)
            {
                if (m % 23 == 0)
                {
                    playerScores[currentPlayer-1] += m;
                    var removeIndex = CounterClockwiseIndexFrom(playedMarbles.Count, currentMarble, 7);
                    playerScores[currentPlayer - 1] += playedMarbles[removeIndex];
                    playedMarbles.RemoveAt(removeIndex);
                    currentMarble = removeIndex;
                }
                else
                {
                    var insertIndex = ClockwiseIndexFrom(playedMarbles.Count, currentMarble, 2);
                    playedMarbles.Insert(insertIndex, m);
                    currentMarble = insertIndex;
                }

                currentPlayer++;
                if (currentPlayer > players)
                    currentPlayer = 1;
            }

            return playerScores.Max();
        }

        private void PrintMarbles(List<int> marbles, int currentMarble, int currentPlayer)
        {
            Console.Write($"[{currentPlayer}]");
            for (var i = 0; i < marbles.Count; i++)
            {
                if (i == currentMarble)
                {
                    Console.Write($"({marbles[i]})".PadLeft(4).PadRight(5));
                }
                else
                {
                    Console.Write($"{marbles[i]}".PadLeft(4).PadRight(5));
                }
            }

            Console.WriteLine();
        }

        private int ClockwiseIndexFrom(int circleCount, int index, int amount)
        {
            if (index + amount >= circleCount)
                return index + amount - circleCount;
            else
                return index + amount;
        }

        private int CounterClockwiseIndexFrom(int circleCount, int index, int amount)
        {
            if (index - amount < 0)
                return index - amount + circleCount;
            else
                return index - amount;
        }
    }
}
