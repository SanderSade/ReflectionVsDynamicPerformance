``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1415 (21H2)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  Job-DDLVLO : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT

Runtime=.NET 6.0  IterationCount=10  LaunchCount=1  
WarmupCount=3  

```
|          Method |     Mean |    Error |   StdDev | Allocated |
|---------------- |---------:|---------:|---------:|----------:|
|        Baseline | 16.78 ms | 0.494 ms | 0.258 ms |     19 KB |
| UsingReflection | 16.03 ms | 0.303 ms | 0.201 ms |     19 KB |
|    UsingDynamic | 16.57 ms | 1.465 ms | 0.872 ms |     19 KB |
