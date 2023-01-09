// <copyright file="ResultTestData.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTests.Helpers;

using Carbonate.BiDirectional;
using Carbonate.UniDirectional;

/// <summary>
/// Used for testing the <see cref="PullReactable{TDataIn,TDataOut}"/> and <see cref="PullReactable{TDataOut}"/> classes.
/// </summary>
public class ResultTestData
{
    /// <summary>
    /// Gets a number.
    /// </summary>
    public int Number { get; init; }
}
