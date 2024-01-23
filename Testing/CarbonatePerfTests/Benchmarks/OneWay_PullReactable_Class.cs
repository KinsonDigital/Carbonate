// <copyright file="OneWay_PullReactable_Class.cs" company="KinsonDigital">
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
public class OneWay_PullReactable_Class
{
    private const string NameSpace = nameof(Carbonate.OneWay);
    private const string IntType = "int";
    private const string StructType = nameof(StructItem);
    private const string PullIntClassName = $"{nameof(PullReactable<int>)}<{IntType}>";
    private const string PullStructClassName = $"{nameof(PullReactable<StructItem>)}<{StructType}>";
    private const string MethodName = $"{nameof(IPullReactable<int>.Pull)}()";
    private readonly Guid setupAId = Guid.NewGuid();
    private readonly Guid setupBId = Guid.NewGuid();
    private PullReactable<int>? pullReactableA;
    private PullReactable<StructItem>? pullReactableB;
    private StructItem structItem;

    [Params(10, 100, 1_000)]
    public int TotalSubscriptions { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        this.pullReactableA = new PullReactable<int>();
        for (var i = 0; i < TotalSubscriptions; i++)
        {
            this.pullReactableA.Subscribe(new RespondSubscription<int>(this.setupAId, () => 123));
        }

        this.structItem = new StructItem { NumberValue = 123 };
        this.pullReactableB = new PullReactable<StructItem>();
        for (var i = 0; i < TotalSubscriptions; i++)
        {
            this.pullReactableB.Subscribe(new RespondSubscription<StructItem>(this.setupAId, () => this.structItem));
        }
    }

    [Benchmark(Description = $"{NameSpace}.{PullIntClassName}.{MethodName}")]
    public void PullReactable_Pull_Method_Setup_A()
    {
        _ = this.pullReactableA.Pull(this.setupAId);
    }

    [Benchmark(Description = $"{NameSpace}.{PullStructClassName}.{MethodName}")]
    public void PullReactable_Pull_Method_Setup_B()
    {
        _ = this.pullReactableB.Pull(this.setupBId);
    }
}
