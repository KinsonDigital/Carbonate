BenchmarkDotNet v0.13.11, Windows 11 (10.0.22631.3007/23H2/2023Update/SunValley3)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.100
[Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2 [AttachedDebugger]
DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


**Description:** Baseline - Standard for loop

| Method                                        | TotalSubscriptions | Mean           | Error        | StdDev       | Median         | Gen0      | Allocated   |
|---------------------------------------------- |------------------- |---------------:|-------------:|-------------:|---------------:|----------:|------------:|
| 'OneWay.PushReactable.Push() Method | int'    | 10                 |       777.2 ns |     15.57 ns |     41.84 ns |       763.8 ns |    0.2480 |     3.05 KB |
| 'OneWay.PushReactable.Push() Method | struct' | 10                 |       490.7 ns |      3.04 ns |      2.70 ns |       490.9 ns |    0.1650 |     2.03 KB |
| 'OneWay.PushReactable.Push() Method | int'    | 100                |    21,234.9 ns |    624.34 ns |  1,811.32 ns |    20,980.2 ns |   19.6838 |   241.41 KB |
| 'OneWay.PushReactable.Push() Method | struct' | 100                |    13,944.0 ns |    641.75 ns |  1,882.16 ns |    12,927.1 ns |   13.1226 |   160.94 KB |
| 'OneWay.PushReactable.Push() Method | int'    | 1000               | 1,361,815.1 ns | 26,895.61 ns | 70,381.13 ns | 1,328,702.9 ns | 1917.9688 | 23507.81 KB |
| 'OneWay.PushReactable.Push() Method | struct' | 1000               |   977,922.6 ns | 14,818.57 ns | 13,861.30 ns |   973,509.8 ns | 1278.3203 | 15671.88 KB |

---

**Description:** Upgrade To CollectionsMarshal.AsSpan()

| Method                                        | TotalSubscriptions | Mean        | Error     | StdDev    | Median      | Allocated |
|---------------------------------------------- |------------------- |------------:|----------:|----------:|------------:|----------:|
| 'OneWay.PushReactable.Push() Method | int'    | 10                 |    15.44 ns |  0.272 ns |  0.415 ns |    15.35 ns |         - |
| 'OneWay.PushReactable.Push() Method | struct' | 10                 |    10.33 ns |  0.216 ns |  0.479 ns |    10.12 ns |         - |
| 'OneWay.PushReactable.Push() Method | int'    | 100                |   123.69 ns |  2.451 ns |  2.724 ns |   123.16 ns |         - |
| 'OneWay.PushReactable.Push() Method | struct' | 100                |    72.17 ns |  1.435 ns |  1.343 ns |    72.36 ns |         - |
| 'OneWay.PushReactable.Push() Method | int'    | 1000               | 1,267.70 ns | 18.864 ns | 17.645 ns | 1,264.68 ns |         - |
| 'OneWay.PushReactable.Push() Method | struct' | 1000               |   752.58 ns | 14.243 ns | 11.894 ns |   751.01 ns |         - |
