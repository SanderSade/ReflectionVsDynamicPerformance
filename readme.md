``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1415 (21H2)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  Job-ZWLRZE : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT

Runtime=.NET 6.0  IterationCount=10  LaunchCount=1  
WarmupCount=3  

```
|          Method |      Mean |      Error |     StdDev | Allocated |
|---------------- |----------:|-----------:|-----------:|----------:|
|        Baseline |  60.36 μs |  13.643 μs |   9.024 μs |         - |
|  SwitchCaseCast |  64.75 μs |   4.898 μs |   2.914 μs |         - |
| UsingReflection | 515.83 μs | 164.164 μs | 108.584 μs |       3 B |
|    UsingDynamic | 256.03 μs |  66.585 μs |  44.042 μs |         - |
