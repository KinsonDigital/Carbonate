BenchmarkDotNet v0.13.11, Windows 11 (10.0.22631.3007/23H2/2023Update/SunValley3)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.100
[Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2 [AttachedDebugger]
DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


**Description:** Baseline - Standard for loop

| Method                                                 | TotalSubscriptions | Mean          | Error         | StdDev        | Gen0      | Allocated  |
|------------------------------------------------------- |------------------- |--------------:|--------------:|--------------:|----------:|-----------:|
| 'TwoWay.PushPullReactable<int, int>.PushPull()'        | 10                 |      66.79 ns |      1.359 ns |      1.565 ns |    0.0248 |      312 B |
| 'TwoWay.PushPullReactable<int, StructItem>.PushPull()' | 10                 |     453.40 ns |      8.811 ns |     13.189 ns |    0.1736 |     2184 B |
| 'TwoWay.PushPullReactable<int, int>.PushPull()'        | 100                |     191.63 ns |      3.814 ns |      7.961 ns |    0.1969 |     2472 B |
| 'TwoWay.PushPullReactable<int, StructItem>.PushPull()' | 100                |  13,615.63 ns |    269.023 ns |    418.837 ns |   13.1989 |   165624 B |
| 'TwoWay.PushPullReactable<int, int>.PushPull()'        | 1000               |   1,439.08 ns |     28.560 ns |     77.214 ns |    1.9169 |    24072 B |
| 'TwoWay.PushPullReactable<int, StructItem>.PushPull()' | 1000               | 947,744.46 ns | 29,045.979 ns | 83,804.291 ns | 1279.2969 | 16056024 B |

---

**Description:** Upgrade To CollectionsMarshal.AsSpan()

| Method                                                 | TotalSubscriptions | Mean       | Error      | StdDev    | Allocated |
|------------------------------------------------------- |------------------- |-----------:|-----------:|----------:|----------:|
| 'TwoWay.PushPullReactable<int, int>.PushPull()'        | 10                 |   2.643 ns |  0.0801 ns | 0.0857 ns |         - |
| 'TwoWay.PushPullReactable<int, StructItem>.PushPull()' | 10                 |   7.184 ns |  0.1688 ns | 0.1579 ns |         - |
| 'TwoWay.PushPullReactable<int, int>.PushPull()'        | 100                |   2.659 ns |  0.0795 ns | 0.0883 ns |         - |
| 'TwoWay.PushPullReactable<int, StructItem>.PushPull()' | 100                |  56.799 ns |  1.0656 ns | 0.9967 ns |         - |
| 'TwoWay.PushPullReactable<int, int>.PushPull()'        | 1000               |   2.671 ns |  0.0802 ns | 0.0955 ns |         - |
| 'TwoWay.PushPullReactable<int, StructItem>.PushPull()' | 1000               | 725.182 ns | 10.1101 ns | 9.4570 ns |         - |
