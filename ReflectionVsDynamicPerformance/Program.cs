using BenchmarkDotNet.Running;
using ReflectionVsDynamicPerformance;

var summary = BenchmarkRunner.Run<Benchmarks>();

Console.WriteLine(summary.AllRuntimes);


Console.ReadKey();