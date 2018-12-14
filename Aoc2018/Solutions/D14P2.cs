using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2018.Solutions
{
    public class D14P2 : Solver<int>
    {
        public override int Day => 14;
        public override int Part => 2;
        protected override string Filename => @"Inputs\d14.input";

        protected override int GetAnswer(string input)
        {
            var searchScores = input.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();
            var elf1 = new ScoreNode(3);
            var elf2 = new ScoreNode(7);

            elf1.Next = elf1.Previous = elf2;
            elf2.Next = elf2.Previous = elf1;

            var lastNode = elf2;
            var scoreCount = 2;

            while (true)
            {
                var scoreSum = elf1.Score + elf2.Score;
                if (scoreSum >= 10)
                {
                    scoreCount++;
                    lastNode = lastNode.InsertAfter(scoreSum / 10 % 10);
                    if (CheckForMatch(lastNode, searchScores))
                        break;
                }

                scoreCount++;
                lastNode = lastNode.InsertAfter(scoreSum % 10);
                if (CheckForMatch(lastNode, searchScores))
                    break;

                elf1 = elf1.Increment(1 + elf1.Score);
                elf2 = elf2.Increment(1 + elf2.Score);
            }

            return scoreCount - input.Length;
        }

        private bool CheckForMatch(ScoreNode lastNode, int[] searchScores)
        {
            var node = lastNode.Decrement(searchScores.Length - 1);
            for (var i = 0; i < searchScores.Length; i++)
                if (node.Score != searchScores[i])
                    return false;
                else
                    node = node.Next;

            return true;
        }

        private class ScoreNode
        {
            public ScoreNode(int score) => Score = score;
            public int Score { get; set; }
            public ScoreNode Next { get; set; }
            public ScoreNode Previous { get; set; }
            public ScoreNode InsertAfter(int n)
            {
                var node = new ScoreNode(n);
                Next.Previous = node;
                node.Next = Next;
                Next = node;
                node.Previous = this;
                return node;
            }
            public ScoreNode Increment(int amount)
            {
                var node = this;
                for (var i = 0; i < amount; i++)
                    node = node.Next;
                return node;
            }
            public ScoreNode Decrement(int amount)
            {
                var node = this;
                for (var i = 0; i < amount; i++)
                    node = node.Previous;
                return node;
            }
        }
    }
}
