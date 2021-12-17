``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1415 (21H2)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  Job-BTATSI : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT

Runtime=.NET 6.0  IterationCount=10  LaunchCount=1  
WarmupCount=3  

```
|          Method |     Mean |    Error |   StdDev | Allocated |
|---------------- |---------:|---------:|---------:|----------:|
|        Baseline | 17.29 ms | 0.571 ms | 0.299 ms |     20 KB |
|  SwitchCaseCast | 16.84 ms | 0.977 ms | 0.581 ms |     19 KB |
| UsingReflection | 16.82 ms | 0.843 ms | 0.558 ms |     19 KB |
|    UsingDynamic | 17.13 ms | 0.376 ms | 0.197 ms |     19 KB |
