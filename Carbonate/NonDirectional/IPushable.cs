// <copyright file="IPushable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.NonDirectional;

/// <summary>
/// Pushes out notifications.
/// </summary>
public interface IPushable
{
    /// <summary>
    /// Pushes a single notification for an event that matches the given <paramref name="eventId"/>.
    /// </summary>
    /// <param name="eventId">The ID of the event where the notification will be pushed.</param>
    void Push(Guid eventId);
}
