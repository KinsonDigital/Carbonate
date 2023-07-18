// <copyright file="IPushable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.UniDirectional;

/// <summary>
/// Pushes out notifications.
/// </summary>
/// <typeparam name="TDataIn">The type of data coming in.</typeparam>
public interface IPushable<TDataIn>
{
    /// <summary>
    /// Pushes a single notification with the given <paramref name="data"/> for an event that matches the given <paramref name="eventId"/>.
    /// </summary>
    /// <param name="data">The data that contains the data to push.</param>
    /// <param name="eventId">The ID of the event where the notification will be pushed.</param>
    void Push(in TDataIn data, Guid eventId);
}
