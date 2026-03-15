using AdventOfCode_2016_CSharp;
using AdventOfCode_2016_CSharp.day01;

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

Console.WriteLine("Advent of Code 2016");
Console.WriteLine($"{"=".Repeat(50)}");

// Day 01
if (Utils.DownloadContent(client, 2016, 1))
{
    //BenchmarkRunner.Run<Day01>();
    Console.WriteLine((new Day01(isTest: false)).SolvePart1());
    Console.WriteLine((new Day01(isTest: false)).SolvePart2());
}