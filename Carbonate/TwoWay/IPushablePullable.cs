﻿// <copyright file="IPushablePullable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.TwoWay;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Gives the ability to push and pull data from a source using a messaging mechanism.
/// </summary>
/// <typeparam name="TIn">The type of data coming from the source.</typeparam>
/// <typeparam name="TOut">The type of data going back to the source.</typeparam>
public interface IPushablePullable<TIn, out TOut>
{
    /// <summary>
    /// Requests to pull data from a source that matches the given <paramref name="id"/>,
    /// with the given additional <paramref name="data"/>.
    /// </summary>
    /// <param name="id">The ID of the response.</param>
    /// <param name="data">The data to send to the responder.</param>
    /// <returns>The data result going out.</returns>
    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Public API.")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "Public API.")]
    TOut? PushPull(Guid id, in TIn data);
}
