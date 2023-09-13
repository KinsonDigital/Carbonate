// <copyright file="PerfScenarios.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable InconsistentNaming
namespace CarbonatePerfTests.MemPerfs;

using Carbonate.OneWay;

/// <summary>
/// Used for testing different scenarios.
/// </summary>
public enum PerfScenarios
{
    /// <summary>
    /// For testing the <see cref="PullReactable{TDataOut}"/>.<see cref="PullReactable{TDataOut}.Pull(System.Guid)"/>
    /// method for <c>struct</c> types.
    /// </summary>
    PullReactable_Pull_Method_With_Struct,

    /// <summary>
    /// For testing the <see cref="PullReactable{TDataOut}"/>.<see cref="PullReactable{TDataOut}.Pull(System.Guid)"/>
    /// method for <see cref="nint"/> types.
    /// </summary>
    PullReactable_Pull_Method_With_Ptr,
}
