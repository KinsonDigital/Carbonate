BenchmarkDotNet v0.13.11, Windows 11 (10.0.22631.3007/23H2/2023Update/SunValley3)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2

**Description:** Baseline - Standard for loop

| Method                              | TotalSubscriptions | Mean        | Error     | StdDev    | Allocated |
|------------------------------------ |------------------- |------------:|----------:|----------:|----------:|
| NonDirectional.PushReactable.Push() | 10                 |    21.11 ns |  0.432 ns |  0.577 ns |         - |
| NonDirectional.PushReactable.Push() | 100                |   183.64 ns |  3.050 ns |  2.704 ns |         - |
| NonDirectional.PushReactable.Push() | 1000               | 2,045.12 ns | 24.447 ns | 21.672 ns |         - |

---

**Description:** Upgrade To CollectionsMarshal.AsSpan()

| Method                              | TotalSubscriptions | Mean        | Error     | StdDev    | Allocated |
|------------------------------------ |------------------- |------------:|----------:|----------:|----------:|
| NonDirectional.PushReactable.Push() | 10                 |    11.89 ns |  0.215 ns |  0.201 ns |         - |
| NonDirectional.PushReactable.Push() | 100                |   112.36 ns |  2.237 ns |  2.747 ns |         - |
| NonDirectional.PushReactable.Push() | 1000               | 1,123.29 ns | 16.086 ns | 15.047 ns |         - |
