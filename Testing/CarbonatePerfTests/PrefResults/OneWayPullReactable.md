BenchmarkDotNet v0.13.11, Windows 11 (10.0.22631.3007/23H2/2023Update/SunValley3)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.100
[Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2 [AttachedDebugger]
DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


**Description:** Baseline - Standard for loop

| Method                                 | TotalSubscriptions | Mean          | Error        | StdDev        | Median        | Gen0     | Allocated |
|--------------------------------------- |------------------- |--------------:|-------------:|--------------:|--------------:|---------:|----------:|
| 'OneWay.PullReactable.Pull() Method | int'    | 10                 |      48.53 ns |     1.004 ns |      2.225 ns |      47.75 ns |   0.0166 |     208 B |
| 'OneWay.PullReactable.Pull() Method | int'    | 100                |     136.20 ns |     3.325 ns |      9.803 ns |     134.40 ns |   0.1311 |    1648 B |
| 'OneWay.PullReactable.Pull() Method | int'    | 1000               |     913.80 ns |    33.283 ns |     98.137 ns |     918.23 ns |   1.2779 |   16048 B |
| 'OneWay.PullReactable.Pull() Method | struct' | 10                 |     261.33 ns |     5.243 ns |     14.875 ns |     256.23 ns |   0.0827 |    1040 B |
| 'OneWay.PullReactable.Pull() Method | struct' | 100                |   6,088.33 ns |   118.878 ns |    211.306 ns |   6,071.15 ns |   6.5613 |   82400 B |
| 'OneWay.PullReactable.Pull() Method | struct' | 1000               | 452,384.60 ns | 9,033.799 ns | 21,469.779 ns | 442,431.15 ns | 639.1602 | 8024000 B |

---

**Description:** Upgrade To CollectionsMarshal.AsSpan()

| Method                                 | TotalSubscriptions | Mean       | Error     | StdDev    | Allocated |
|--------------------------------------- |------------------- |-----------:|----------:|----------:|----------:|
| 'OneWay.PullReactable.Pull() Method | int'    | 10                 |   3.012 ns | 0.0928 ns | 0.1032 ns |         - |
| 'OneWay.PullReactable.Pull() Method | int'    | 100                |   2.594 ns | 0.0816 ns | 0.1271 ns |         - |
| 'OneWay.PullReactable.Pull() Method | int'    | 1000               |   2.670 ns | 0.0767 ns | 0.0970 ns |         - |
| 'OneWay.PullReactable.Pull() Method | struct' | 10                 |   6.637 ns | 0.1521 ns | 0.3952 ns |         - |
| 'OneWay.PullReactable.Pull() Method | struct' | 100                |  65.871 ns | 1.3041 ns | 1.2808 ns |         - |
| 'OneWay.PullReactable.Pull() Method | struct' | 1000               | 775.474 ns | 4.2197 ns | 3.7406 ns |         - |
