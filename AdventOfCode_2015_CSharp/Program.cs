using AdventOfCode_2015_CSharp;
using AdventOfCode_2015_CSharp.day00;
using AdventOfCode_2015_CSharp.day01;
using AdventOfCode_2015_CSharp.day02;
using AdventOfCode_2015_CSharp.day03;
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

//// Day 00
//if (Utils.DownloadContent(client, 2015, 0))
//{
//    //BenchmarkRunner.Run<Day00>();
//    Console.WriteLine((new Day00(isTest: true)).SolvePart1());
//    Console.WriteLine((new Day00(isTest: true)).SolvePart2());
//}

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