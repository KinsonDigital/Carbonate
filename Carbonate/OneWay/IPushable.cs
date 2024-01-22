// <copyright file="IPushable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.OneWay;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Pushes out notifications.
/// </summary>
/// <typeparam name="TIn">The type of data coming from the source.</typeparam>
public interface IPushable<TIn>
{
    /// <summary>
    /// Pushes a single notification with the given <paramref name="data"/> for an event that matches the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The ID of the event where the notification will be pushed.</param>
    /// <param name="data">The data that contains the data to push.</param>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "Public API.")]
    void Push(Guid id, in TIn data);
}
