// <copyright file="StructDataPuller.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonatePerfTests;

using Carbonate.UniDirectional;

/// <summary>
/// Used for per testing.
/// </summary>
public class StructDataPuller
{
    private readonly IPullReactable<StructItem[]> pullReactable;

    public StructDataPuller(IPullReactable<StructItem[]> pullReactable) => this.pullReactable = pullReactable;

    public StructItem[]? Pull()
    {
        return this.pullReactable.Pull(Ids.GetDatId);
    }
}
