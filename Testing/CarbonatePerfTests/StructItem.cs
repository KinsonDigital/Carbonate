// <copyright file="StructItem.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace CarbonatePerfTests;

/// <summary>
/// Used for perf testing.
/// </summary>
public readonly struct StructItem
{
    public int NumberValue { get; init; }

    public string StringValue { get; init; }
}
