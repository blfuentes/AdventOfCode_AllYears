using AdventOfCode_2018_CSharp;
using AdventOfCode_2018_CSharp.day01;
using AdventOfCode_2018_CSharp.day02;
using AdventOfCode_2018_CSharp.day03;
using AdventOfCode_2018_CSharp.day04;
using AdventOfCode_2018_CSharp.day05;
using AdventOfCode_2018_CSharp.day06;
using AdventOfCode_2018_CSharp.day07;
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

Console.WriteLine("Advent of Code 2018");
Console.WriteLine($"{"=".Repeat(50)}");

// Day 01
if (Utils.DownloadContent(client, 2018, 1))
{
    //BenchmarkRunner.Run<Day01>();
    Console.WriteLine((new Day01(isTest: false)).SolvePart1());
    Console.WriteLine((new Day01(isTest: false)).SolvePart2());
}

// Day 02
if (Utils.DownloadContent(client, 2018, 2))
{
    //BenchmarkRunner.Run<Day02>();
    Console.WriteLine((new Day02(isTest: false)).SolvePart1());
    Console.WriteLine((new Day02(isTest: false)).SolvePart2());
}

// Day 03
if (Utils.DownloadContent(client, 2018, 3))
{
    //BenchmarkRunner.Run<Day03>();
    Console.WriteLine((new Day03(isTest: false)).SolvePart1());
    Console.WriteLine((new Day03(isTest: false)).SolvePart2());
}

// Day 04
if (Utils.DownloadContent(client, 2018, 4))
{
    //BenchmarkRunner.Run<Day04>();
    Console.WriteLine((new Day04(isTest: false)).SolvePart1());
    Console.WriteLine((new Day04(isTest: false)).SolvePart2());
}

// Day 05
if (Utils.DownloadContent(client, 2018, 5))
{
    //BenchmarkRunner.Run<Day05>();
    Console.WriteLine((new Day05(isTest: false)).SolvePart1());
    Console.WriteLine((new Day05(isTest: false)).SolvePart2());
}

// Day 06
if (Utils.DownloadContent(client, 2018, 6))
{
    //BenchmarkRunner.Run<Day06>();
    Console.WriteLine((new Day06(isTest: false)).SolvePart1());
    Console.WriteLine((new Day06(isTest: false)).SolvePart2());
}

// Day 07
if (Utils.DownloadContent(client, 2018, 7))
{
    //BenchmarkRunner.Run<Day07>();
    Console.WriteLine((new Day07(isTest: true)).SolvePart1());
    Console.WriteLine((new Day07(isTest: true)).SolvePart2());
}