``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1415 (21H2)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  Job-KOQBBW : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT

Runtime=.NET 6.0  IterationCount=10  LaunchCount=1  
WarmupCount=3  

```
|                Method |      Mean |     Error |    StdDev | Allocated |
|---------------------- |----------:|----------:|----------:|----------:|
|              Baseline |  50.60 μs |  1.049 μs |  0.694 μs |         - |
|        SwitchCaseCast |  63.06 μs |  2.410 μs |  1.434 μs |         - |
|       UsingReflection | 359.25 μs | 19.278 μs | 11.472 μs |       3 B |
| ReflectionWithCaching | 319.43 μs | 12.787 μs |  8.458 μs |         - |
|          UsingDynamic | 208.80 μs |  9.763 μs |  5.810 μs |         - |
