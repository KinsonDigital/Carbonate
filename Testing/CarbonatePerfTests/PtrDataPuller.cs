// <copyright file="PtrDataPuller.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonatePerfTests;

using System.Runtime.CompilerServices;
using Carbonate.OneWay;

/// <summary>
/// Used for perf testing.
/// </summary>
public class PtrDataPuller
{
    private readonly IPullReactable<nint> pullReactable;

    public PtrDataPuller(IPullReactable<nint> pullReactable) => this.pullReactable = pullReactable;

    public Span<StructItem> Pull()
    {
        var result = this.pullReactable.Pull(Ids.GetDatId);

        unsafe
        {
            return Unsafe.AsRef<Memory<StructItem>>(result.ToPointer()).Span;
        }
    }
}
