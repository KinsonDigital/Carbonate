// <copyright file="NonDir_PushReactable_Class.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonatePerfTests.Benchmarks;

using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using Carbonate.NonDirectional;

[MemoryDiagnoser]
[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Perf testing.")]
[SuppressMessage("csharpsquid", "S101", Justification = "Perf testing")]
public class NonDir_PushReactable_Class
{
    private const string NameSpace = nameof(Carbonate.NonDirectional);
    private const string ClassName = nameof(PushReactable);
    private const string MethodName = $"{nameof(PushReactable.Push)}()";
    private readonly Guid setupAId = Guid.NewGuid();
    private PushReactable? pushReactable;

    [Params(10, 100, 1_000)]
    public int TotalSubscriptions { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        this.pushReactable = new PushReactable();
        for (var i = 0; i < TotalSubscriptions; i++)
        {
            this.pushReactable.Subscribe(new ReceiveSubscription(this.setupAId, () => { }));
        }
    }

    [Benchmark(Description = $"{NameSpace}.{ClassName}.{MethodName}")]
    public void PushReactable_Push_Method_Setup()
    {
        this.pushReactable.Push(this.setupAId);
    }
}
