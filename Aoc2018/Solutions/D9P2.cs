using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2018.Solutions
{
    public class D9P2 : Solver<BigInteger>
    {
        public override int Day => 9;

        public override int Part => 2;

        protected override string Filename => @"Inputs\d9p1.input";

        protected override BigInteger GetAnswer(string input)
        {
            var regex = new Regex(@"(\d+) players; last marble is worth (\d+) points");
            var match = regex.Match(input);
            var players = int.Parse(match.Groups[1].ToString());
            var marbles = 100 * int.Parse(match.Groups[2].ToString());

            var playerScores = new BigInteger[players];

            var currentPlayer = 0;
            var currentMarble = new Node<int>(0);
            currentMarble.Next = currentMarble;
            currentMarble.Previous = currentMarble;

            for (var m = 1; m <= marbles; m++)
            {
                if (m % 23 == 0)
                {
                    var removedMarble = RemoveMarbleForGame(currentMarble);
                    currentMarble = removedMarble.Next;

                    playerScores[currentPlayer] += removedMarble.Value + m;
                }
                else
                {
                    currentMarble = AddMarbleForGame(currentMarble, m);
                }

                currentPlayer++;
                if (currentPlayer >= players)
                    currentPlayer = 0;
            }

            return playerScores.Max();
        }

        private Node<int> AddMarbleForGame(Node<int> currentMarble, int marble)
        {
            return currentMarble.Next.InsertAfter(marble);
        }

        private Node<int> RemoveMarbleForGame(Node<int> currentMarble)
        {
            for (var i = 0; i < 7; i++)
            {
                currentMarble = currentMarble.Previous;
            }

            currentMarble.RemoveSelf();

            return currentMarble;
        }

        private class Node<T>
        {
            public Node(T value) => Value = value;
            public Node<T> Next { get; set; }
            public Node<T> Previous { get; set; }
            public T Value { get; set; }
            public Node<T> InsertAfter(T value)
            {
                var node = new Node<T>(value);
                Next.Previous = node;
                node.Next = Next;
                Next = node;
                node.Previous = this;
                return node;
            }

            public void RemoveSelf()
            {
                Previous.Next = Next;
                Next.Previous = Previous;
            }
        }
    }
}
