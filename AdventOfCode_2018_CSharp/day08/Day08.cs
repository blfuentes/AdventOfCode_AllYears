using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis;

namespace AdventOfCode_2018_CSharp.day08;

public class Day08 : BaseDay
{
    private readonly int[] _numbers;

    struct Node
    {
        public int Id;
        public Node[] Children;
        public int[] Metadata;

        public readonly int NodeValue()
        {
            if (Children.Length == 0)
            {
                return Metadata.Sum();
            }
            else
            {
                int sum = 0;
                foreach (var metadata in Metadata)
                {
                    if (metadata > Children.Length)
                        continue;

                    sum += Children[metadata - 1].NodeValue();
                }
                return sum;
            }
        }
    }

    public Day08(bool isTest = false) : base("08", isTest)
    {
        _numbers = [.. Content.Split(" ").Select(int.Parse)];
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        Node tree = new();

        int startIdx = 0;
        void BuildTree(ref Node node)
        {
            if (startIdx < _numbers.Length)
            {
                var childrenSize = _numbers[startIdx++];
                var metadataSize = _numbers[startIdx++];

                node.Id = startIdx;
                node.Children = new Node[childrenSize];
                node.Metadata = new int[metadataSize];

                for (int c = 0; c < childrenSize; c++)
                {
                    Node child = new();
                    BuildTree(ref child);
                    node.Children[c] = child;
                }
                int counter = 0;
                while (counter < metadataSize)
                {
                    node.Metadata[counter++] = _numbers[startIdx++];
                }
            }
        }
        BuildTree(ref tree);

        int sum = 0;
        Stack<Node> nodes = [];
        nodes.Push(tree);
        while (nodes.TryPop(out Node n))
        {
            if (n.Metadata is not null)
                sum += n.Metadata.Sum();
            if (n.Children is not null)
            {
                foreach (var child in n.Children)
                    nodes.Push(child);
            }
        }

        return sum;
    }

    public override string SolvePart1()
    {
        StopWatch.Start();
        var result = RunPart1();
        StopWatch.Stop();
        return $"Final result Day {Day} part 1: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }
    #endregion

    #region Part 2
    [Benchmark]
    public int RunPart2()
    {
        Node tree = new();

        int startIdx = 0;
        void BuildTree(ref Node node)
        {
            if (startIdx < _numbers.Length)
            {
                var childrenSize = _numbers[startIdx++];
                var metadataSize = _numbers[startIdx++];

                node.Id = startIdx;
                node.Children = new Node[childrenSize];
                node.Metadata = new int[metadataSize];

                for (int c = 0; c < childrenSize; c++)
                {
                    Node child = new();
                    BuildTree(ref child);
                    node.Children[c] = child;
                }
                int counter = 0;
                while (counter < metadataSize)
                {
                    node.Metadata[counter++] = _numbers[startIdx++];
                }
            }
        }
        BuildTree(ref tree);

        return tree.NodeValue();
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }
    #endregion
}
