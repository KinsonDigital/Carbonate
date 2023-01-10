// <copyright file="IReceiver.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.UniDirectional;

/// <summary>
/// Gives the ability to receive push notifications.
/// </summary>
/// <typeparam name="TDataIn">The type of data packaged in the <see cref="IMessage{TDataIn}"/>.</typeparam>
public interface IReceiver<in TDataIn>
{
    /// <summary>
    /// Gets a notification of an event with the given <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The notification message.</param>
    void OnReceive(IMessage<TDataIn> message);
}
