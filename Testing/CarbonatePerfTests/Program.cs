﻿// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable SA1200
// ReSharper disable RedundantUsingDirective
// ReSharper disable JoinDeclarationAndInitializer
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using CarbonatePerfTests.Benchmarks;

#if RELEASE_NONDIRPUSHREACTABLE || RELEASE_ONEWAYPULLREACTABLE || RELEASE_ONEWAYPUSHREACTABLE || RELEASE_TWOWAYPUSHPULLREACTABLE
Summary? summary;
#endif

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

#elif RELEASE_NONDIRPUSHREACTABLE

summary = BenchmarkRunner.Run<NonDir_PushReactable_Class>();

#elif RELEASE_ONEWAYPULLREACTABLE

summary = BenchmarkRunner.Run<OneWay_PullReactable_Class>();

#elif RELEASE_ONEWAYPUSHREACTABLE

summary = BenchmarkRunner.Run<OneWay_PushReactable_Class>();

#elif RELEASE_TWOWAYPUSHPULLREACTABLE

summary = BenchmarkRunner.Run<TwoWay_PushPullReactable_Class>();

#endif

#if RELEASE_NONDIRPUSHREACTABLE || RELEASE_ONEWAYPULLREACTABLE || RELEASE_ONEWAYPUSHREACTABLE || RELEASE_TWOWAYPUSHPULLREACTABLE
Console.WriteLine(summary);
#endif

Console.ReadLine();
