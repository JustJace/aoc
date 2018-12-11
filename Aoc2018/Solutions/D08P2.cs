using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2018.Solutions
{
    public class D08P2 : Solver<int>
    {
        public override int Day => 8;

        public override int Part => 2;

        protected override string Filename => @"Inputs\d08.input";

        protected override int GetAnswer(string input)
        {
            var license = input.Split(' ').Select(int.Parse).ToArray();

            var root = new Node() { };
            var processStack = new Stack<Node>();
            processStack.Push(root);

            var i = 0;
            while (processStack.Any())
            {
                var node = processStack.Pop();

                if (node.ChildCount.HasValue)
                {
                    for (var m = 0; m < node.MetadataCount; m++)
                    {
                        var metadata = license[i++];
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

            return NodeValueRecursive(root);
        }

        private int NodeValueRecursive(Node node)
        {
            if (node.Value.HasValue)
                return node.Value.Value;

            if (node.ChildCount.Value == 0)
            {
                node.Value = node.Metadata.Sum();
                return node.Value.Value;
            }
            else
            {
                var value = 0;

                foreach (var metadata in node.Metadata)
                {
                    if (metadata != 0 && metadata - 1 < node.ChildCount.Value)
                    {
                        value += NodeValueRecursive(node.Children[node.ChildCount.Value - metadata]);
                    }
                }

                node.Value = value;
                return node.Value.Value;
            }
        }

        private class Node
        {
            public int? ChildCount { get; set; }
            public int MetadataCount { get; set; }
            public List<Node> Children { get; } = new List<Node>();
            public List<int> Metadata { get; } = new List<int>();
            public int? Value { get; set; }
            public override string ToString()
            {
                return Value.ToString();
            }
        }
    }
}
