// <copyright file="IReceiver.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

/// <summary>
/// Gives the ability to receive push notifications.
/// </summary>
public interface IReceiver
{
    /// <summary>
    /// Gets a notification of an event.
    /// </summary>
    void OnReceive();

    /// <summary>
    /// Gets a notification of an event with the given <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The notification message.</param>
    void OnReceive(IMessage message);
}
