using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2018.Solutions
{
    public class D14P1 : Solver<string>
    {
        public override int Day => 14;
        public override int Part => 1;
        protected override string Filename => @"Inputs\d14.input";

        protected override string GetAnswer(string input)
        {
            var n = int.Parse(input);
            var elf1 = new ScoreNode(3);
            var elf2 = new ScoreNode(7);

            elf1.Next = elf1.Previous = elf2;
            elf2.Next = elf2.Previous = elf1;

            var lastNode = elf2;
            var scoreCount = 2;

            while (scoreCount < n + 10)
            {
                var scoreSum = elf1.Score + elf2.Score;
                if (scoreSum >= 10)
                {
                    lastNode = lastNode.InsertAfter(scoreSum / 10 % 10);
                    scoreCount++;
                }

                lastNode = lastNode.InsertAfter(scoreSum % 10);
                scoreCount++;

                elf1 = elf1.Increment(1 + elf1.Score);
                elf2 = elf2.Increment(1 + elf2.Score);
            }

            var node = lastNode;
            if (scoreCount > n + 10)
                node = node.Previous;

            node = node.Decrement(9);

            var answer = "";

            for (var i = 0; i < 10; i++)
            {
                answer += node.Score;
                node = node.Next;
            }

            return answer;
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
