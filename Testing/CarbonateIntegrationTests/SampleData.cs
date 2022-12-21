// <copyright file="SampleData.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateIntegrationTests;

/// <summary>
/// Holds sample data for the purpose of integration testing.
/// </summary>
public class SampleData
{
    /// <summary>
    /// Gets the sample <c>string</c> value.
    /// </summary>
    public string StringValue { get; init; } = string.Empty;

    /// <summary>
    /// Gets the sample <c>int</c> value.
    /// </summary>
    public int IntValue { get; init; }
}
