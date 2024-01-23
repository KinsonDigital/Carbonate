// <copyright file="OneWay_PushReactable_Class.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace CarbonatePerfTests.Benchmarks;

using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using Carbonate.OneWay;

[MemoryDiagnoser]
[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Perf testing.")]
[SuppressMessage("csharpsquid", "S101", Justification = "Perf testing")]
public class OneWay_PushReactable_Class
{
    private const string NameSpace = nameof(Carbonate.OneWay);
    private const string IntType = "int";
    private const string StructType = nameof(StructItem);
    private const string PushIntClassName = $"{nameof(PushReactable<int>)}<{IntType}>";
    private const string PushStructClassName = $"{nameof(PushReactable<StructItem>)}<{StructType}>";
    private const string MethodName = $"{nameof(IPushReactable<int>.Push)}()";
    private readonly Guid setupAId = Guid.NewGuid();
    private readonly Guid setupBId = Guid.NewGuid();
    private PushReactable<int>? pushReactableA;
    private PushReactable<StructItem>? pushReactableB;
    private StructItem structItem;

    [Params(10, 100, 1_000)]
    public int TotalSubscriptions { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        this.pushReactableA = new PushReactable<int>();
        for (var i = 0; i < TotalSubscriptions; i++)
        {
            this.pushReactableA.Subscribe(new ReceiveSubscription<int>(this.setupAId, _ => { }));
        }

        this.structItem = new StructItem { NumberValue = 123 };
        this.pushReactableB = new PushReactable<StructItem>();
        for (var i = 0; i < TotalSubscriptions; i++)
        {
            this.pushReactableB.Subscribe(new ReceiveSubscription<StructItem>(this.setupAId, _ => { }));
        }
    }

    [Benchmark(Description = $"{NameSpace}.{PushIntClassName}.{MethodName}")]
    public void PushReactable_Push_Method_Setup_A()
    {
        this.pushReactableA.Push(this.setupAId, 123);
    }

    [Benchmark(Description = $"{NameSpace}.{PushStructClassName}.{MethodName}")]
    public void PushReactable_Push_Method_Setup_B()
    {
        this.pushReactableB.Push(this.setupBId, this.structItem);
    }
}
