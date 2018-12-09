using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2018.Solutions
{
    public class D8P1 : Solver<int>
    {
        public override int Day => 8;

        public override int Part => 1;

        protected override string Filename => @"Inputs\d8p1.input";

        protected override int GetAnswer(string input)
        {
            var license = input.Split(' ').Select(int.Parse).ToArray();

            var root = new Node() { };
            var processStack = new Stack<Node>();
            processStack.Push(root);

            var i = 0;
            var metadataTotal = 0;
            while (processStack.Any())
            {
                var node = processStack.Pop();

                if (node.ChildCount.HasValue)
                {
                    for (var m = 0; m < node.MetadataCount; m++)
                    {
                        var metadata = license[i++];
                        metadataTotal += metadata;
                        node.Metadata.Add(metadata);
                    }
                }
                else
                {
                    node.ChildCount = license[i++];
                    node.MetadataCount = license[i++];
                    processStack.Push(node);

                    for (var c = 0; c < node.ChildCount; c++)
                    {
                        var cNode = new Node();
                        node.Children.Add(cNode);
                        processStack.Push(cNode);
                    }
                }
            }

            return metadataTotal;
        }

        private class Node
        {
            public int? ChildCount { get; set; }
            public int MetadataCount { get; set; }
            public List<Node> Children { get; } = new List<Node>();
            public List<int> Metadata { get; } = new List<int>();
        }
    }
}
