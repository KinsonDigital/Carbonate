// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonatePerfTests;

// ReSharper disable RedundantUsingDirective
using BenchmarkDotNet.Running;
using CarbonatePerfTests;
using Benchmarks;
using MemPerfs;

// ReSharper restore RedundantUsingDirective
internal static class Program
{
    public static void Main()
    {
#if DEBUG
        Console.WriteLine("Add simple debugging code and manual testing here.");

#elif RELEASE

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("WARNING!!");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("This application is for testing purposes only.");
        Console.WriteLine("This can only be run with the following solution configuartions:");
        Console.WriteLine("\t- Release Benchmark");
        Console.WriteLine("\t- Release MemPerf");

#elif RELEASE_BENCHMARK

        var summary = BenchmarkRunner.Run<PullReactable_Class>();
        Console.WriteLine(summary);

#elif RELEASE_MEMPERF

        // MemPerfRunner.Run(PerfScenarios.PullReactable_Pull_Method_With_Struct, -1);
        MemPerfRunner.Run(PerfScenarios.PullReactable_Pull_Method_With_Ptr, -1);

#endif
        Console.ReadLine();
    }
}
