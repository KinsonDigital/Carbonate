// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable RedundantUsingDirective
using System.Runtime.InteropServices;
using BenchmarkDotNet.Running;
using Carbonate.OneWay;
using CarbonatePerfTests.Benchmarks;

#if DEBUG

Console.WriteLine("Add simple debugging code and manual testing here.");

#elif RELEASE

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("WARNING!!");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("This application is for testing purposes only.");
Console.WriteLine("This can only be run with the following solution configurations:");
Console.WriteLine("\t- PullReactable");
Console.WriteLine("\t- PushReactable");

#elif RELEASE_ONEWAYPULLREACTABLE

var summary = BenchmarkRunner.Run<PullReactable_Class>();
Console.WriteLine(summary);

#elif RELEASE_ONEWAYPUSHREACTABLE

var summary = BenchmarkRunner.Run<OneWay_PushReactable_Class>();
Console.WriteLine(summary);

#endif

Console.ReadLine();
