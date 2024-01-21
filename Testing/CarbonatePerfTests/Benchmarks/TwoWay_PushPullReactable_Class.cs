// <copyright file="TwoWay_PushPullReactable_Class.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace CarbonatePerfTests.Benchmarks;

using BenchmarkDotNet.Attributes;
using System.Diagnostics.CodeAnalysis;
using Carbonate.TwoWay;

[MemoryDiagnoser]
[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Perf testing.")]
[SuppressMessage("csharpsquid", "S101", Justification = "Perf testing")]
public class TwoWay_PushPullReactable_Class
{
    private const string NameSpace = nameof(Carbonate.TwoWay);
    private const string IntType = "int";
    private const string StructType = nameof(StructItem);
    private const string PushIntPullIntClassName = $"{nameof(PushPullReactable<int, int>)}<{IntType}, {IntType}>";
    private const string PushIntPullStructClassName = $"{nameof(PushPullReactable<int, StructItem>)}<{IntType}, {StructType}>";
    private const string MethodName = $"{nameof(IPushPullReactable<int, StructItem>.PushPull)}()";
    private readonly Guid setupAId = Guid.NewGuid();
    private readonly Guid setupBId = Guid.NewGuid();
    private PushPullReactable<int, int>? pushPullReactableA;
    private PushPullReactable<int, StructItem>? pushPullReactableB;
    private StructItem structItem;

    [Params(10, 100, 1_000)]
    public int TotalSubscriptions { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        this.pushPullReactableA = new PushPullReactable<int, int>();
        for (var i = 0; i < TotalSubscriptions; i++)
        {
            this.pushPullReactableA.Subscribe(new ReceiveRespondSubscription<int, int>(this.setupAId, _ => 123));
        }

        this.structItem = new StructItem { NumberValue = 456 };
        this.pushPullReactableB = new PushPullReactable<int, StructItem>();
        for (var i = 0; i < TotalSubscriptions; i++)
        {
            this.pushPullReactableB.Subscribe(new ReceiveRespondSubscription<int, StructItem>(this.setupAId, _ => this.structItem));
        }
    }

    [Benchmark(Description = $"{NameSpace}.{PushIntPullIntClassName}.{MethodName}")]
    public void PullReactable_Pull_Method_Setup_A()
    {
        _ = this.pushPullReactableA.PushPull(10, this.setupAId);
    }

    [Benchmark(Description = $"{NameSpace}.{PushIntPullStructClassName}.{MethodName}")]
    public void PullReactable_Pull_Method_Setup_B()
    {
        _ = this.pushPullReactableB.PushPull(20, this.setupBId);
    }
}
