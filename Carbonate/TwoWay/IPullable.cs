// <copyright file="IPullable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.TwoWay;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Gives the ability to pull data from a source using a messaging mechanism.
/// </summary>
/// <typeparam name="TDataIn">The type of data coming in.</typeparam>
/// <typeparam name="TDataOut">The type of data going out.</typeparam>
public interface IPullable<TDataIn, out TDataOut>
{
    /// <summary>
    /// Requests to pull data from a source that matches the given <paramref name="respondId"/>,
    /// with the given additional <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The data to send to the responder.</param>
    /// <param name="respondId">The ID of the response.</param>
    /// <returns>The data result going out.</returns>
    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Public API.")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "Public API.")]
    TDataOut? Pull(in TDataIn data, Guid respondId);
}
