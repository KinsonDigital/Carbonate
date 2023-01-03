// <copyright file="IPushable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

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

    /// <summary>
    /// Pushes a single notification with the given <paramref name="data"/> for an event that matches the given <paramref name="eventId"/>.
    /// </summary>
    /// <param name="data">The data to send with the push notification.</param>
    /// <param name="eventId">The ID of the event where the notification will be pushed.</param>
    /// <typeparam name="T">The type of data to push.</typeparam>
    void PushData<T>(in T data, Guid eventId);

    /// <summary>
    /// Pushes a single notification with the given <paramref name="message"/> for an event that matches the given <paramref name="eventId"/>.
    /// </summary>
    /// <param name="message">The message that contains the data to push.</param>
    /// <param name="eventId">The ID of the event where the notification will be pushed.</param>
    void PushMessage(in IMessage message, Guid eventId);
}
