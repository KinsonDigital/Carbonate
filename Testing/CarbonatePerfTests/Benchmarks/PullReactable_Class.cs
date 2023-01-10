// <copyright file="PullReactable_Class.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonatePerfTests.Benchmarks;

using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Carbonate.UniDirectional;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Only used for testing.")]
public class PullReactable_Class
{
    // ReSharper disable NotAccessedField.Local
    private PullReactable<StructItem[]>? pullReactableA;
    private PullReactable<nint>? pullReactableB;
    private StructDataStore? structDataStore;
    private StructDataPuller? structDataPuller;
    private PtrDataStore? ptrDataStore;
    private PtrDataPuller? ptrDataPuller;

    // ReSharper restore NotAccessedField.Local
    [GlobalSetup]
    public void GlobalSetup()
    {
        this.pullReactableA = new PullReactable<StructItem[]>();
        this.pullReactableB = new PullReactable<nint>();

        this.structDataStore = new StructDataStore(this.pullReactableA);
        this.structDataPuller = new StructDataPuller(this.pullReactableA);

        this.ptrDataStore = new PtrDataStore(this.pullReactableB);
        this.ptrDataPuller = new PtrDataPuller(this.pullReactableB);
    }

    [Benchmark(Description = "PullReactable.Pull() Method | Setup A")]
    public void PullReactable_Pull_Method_Setup_A()
    {
        _ = this.structDataPuller.Pull();
    }

    [Benchmark(Description = "PullReactable.Pull() Method | Setup B")]
    public void PullReactable_Pull_Method_Setup_B()
    {
        _ = this.ptrDataPuller.Pull();
    }
}
