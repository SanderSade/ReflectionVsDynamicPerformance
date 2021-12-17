using BenchmarkDotNet.Running;
using ReflectionVsDynamicPerformance;

var summary = BenchmarkRunner.Run<BenchMarks>();

Console.WriteLine(summary.AllRuntimes);


Console.ReadKey();