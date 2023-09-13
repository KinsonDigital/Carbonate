// <copyright file="IReceiver.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.OneWay;

/// <summary>
/// Gives the ability to receive push notifications.
/// </summary>
/// <typeparam name="TIn">The type of data coming in.</typeparam>
public interface IReceiver<in TIn>
{
    /// <summary>
    /// Gets a notification of an event with the given <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The notification data.</param>
    void OnReceive(TIn data);
}
