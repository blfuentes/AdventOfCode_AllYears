using AdventOfCode_2015_CSharp;
using AdventOfCode_2015_CSharp.day01;
using AdventOfCode_2015_CSharp.day02;
using AdventOfCode_2015_CSharp.day03;
using AdventOfCode_2015_CSharp.day04;
using AdventOfCode_2015_CSharp.day05;
using AdventOfCode_2015_CSharp.day06;
using AdventOfCode_2015_CSharp.day07;
using AdventOfCode_2015_CSharp.day08;
using AdventOfCode_2015_CSharp.day09;
using AdventOfCode_2015_CSharp.day10;
using AdventOfCode_2015_CSharp.day11;
using AdventOfCode_2015_CSharp.day12;
using AdventOfCode_2015_CSharp.day13;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddHttpClient("AdventOfCodeClient", client =>
{
    client.BaseAddress = new Uri("https://adventofcode.com/");
    client.DefaultRequestHeaders.Add("Accept", "text/plain");
});
var serviceProvider = services.BuildServiceProvider();
var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
var client = httpClientFactory.CreateClient("AdventOfCodeClient");

Console.WriteLine("Advent of Code 215");
Console.WriteLine($"{"=".Repeat(50)}");

// Day 01
if (Utils.DownloadContent(client, 2015, 1))
{
    //BenchmarkRunner.Run<Day01>();
    Console.WriteLine((new Day01(isTest: false)).SolvePart1());
    Console.WriteLine((new Day01(isTest: false)).SolvePart2());
}

// Day 02
if (Utils.DownloadContent(client, 2015, 2))
{
    //BenchmarkRunner.Run<Day02>();
    Console.WriteLine((new Day02(isTest: false)).SolvePart1());
    Console.WriteLine((new Day02(isTest: false)).SolvePart2());
}

// Day 03
if (Utils.DownloadContent(client, 2015, 3))
{
    //BenchmarkRunner.Run<Day03>();
    Console.WriteLine((new Day03(isTest: false)).SolvePart1());
    Console.WriteLine((new Day03(isTest: false)).SolvePart2());
}

// Day 04
if (Utils.DownloadContent(client, 2015, 4))
{
    //BenchmarkRunner.Run<Day04>();
    Console.WriteLine((new Day04(isTest: false)).SolvePart1());
    Console.WriteLine((new Day04(isTest: false)).SolvePart2());
}

// Day 05
if (Utils.DownloadContent(client, 2015, 5))
{
    //BenchmarkRunner.Run<Day05>();
    Console.WriteLine((new Day05(isTest: false)).SolvePart1());
    Console.WriteLine((new Day05(isTest: false)).SolvePart2());
}

// Day 06
if (Utils.DownloadContent(client, 2015, 6))
{
    //BenchmarkRunner.Run<Day06>();
    Console.WriteLine((new Day06(isTest: false)).SolvePart1());
    Console.WriteLine((new Day06(isTest: false)).SolvePart2());
}

// Day 07
if (Utils.DownloadContent(client, 2015, 7))
{
    //BenchmarkRunner.Run<Day07>();
    Console.WriteLine((new Day07(isTest: false)).SolvePart1());
    Console.WriteLine((new Day07(isTest: false)).SolvePart2());
}

// Day 08
if (Utils.DownloadContent(client, 2015, 8))
{
    //BenchmarkRunner.Run<Day08>();
    Console.WriteLine((new Day08(isTest: false)).SolvePart1());
    Console.WriteLine((new Day08(isTest: false)).SolvePart2());
}

// Day 09
if (Utils.DownloadContent(client, 2015, 9))
{
    //BenchmarkRunner.Run<Day09>();
    Console.WriteLine((new Day09(isTest: false)).SolvePart1());
    Console.WriteLine((new Day09(isTest: false)).SolvePart2());
}

// Day 10
if (Utils.DownloadContent(client, 2015, 10))
{
    //BenchmarkRunner.Run<Day10>();
    Console.WriteLine((new Day10(isTest: false)).SolvePart1());
    Console.WriteLine((new Day10(isTest: false)).SolvePart2());
}

// Day 11
if (Utils.DownloadContent(client, 2015, 11))
{
    //BenchmarkRunner.Run<Day11>();
    Console.WriteLine((new Day11(isTest: false)).SolvePart1());
    Console.WriteLine((new Day11(isTest: false)).SolvePart2());
}

// Day 12
if (Utils.DownloadContent(client, 2015, 12))
{
    //BenchmarkRunner.Run<Day12>();
    Console.WriteLine((new Day12(isTest: false)).SolvePart1());
    Console.WriteLine((new Day12(isTest: false)).SolvePart2());
}

// Day 13
if (Utils.DownloadContent(client, 2015, 13))
{
    //BenchmarkRunner.Run<Day13>();
    Console.WriteLine((new Day13(isTest: false)).SolvePart1());
    Console.WriteLine((new Day13(isTest: false)).SolvePart2());
}