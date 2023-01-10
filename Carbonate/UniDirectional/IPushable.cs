// <copyright file="IPushable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.UniDirectional;

using Core;

/// <summary>
/// Pushes out notifications.
/// </summary>
/// <typeparam name="TDataIn">The type of data in the <see cref="IMessage{TDataOut}"/>.</typeparam>
public interface IPushable<TDataIn>
{
    /// <summary>
    /// Pushes a single notification with the given <paramref name="message"/> for an event that matches the given <paramref name="eventId"/>.
    /// </summary>
    /// <param name="message">The message that contains the data to push.</param>
    /// <param name="eventId">The ID of the event where the notification will be pushed.</param>
    void PushMessage(in IMessage<TDataIn> message, Guid eventId);
}
