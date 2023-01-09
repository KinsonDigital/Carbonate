// <copyright file="IReceive.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.NonDirectional;

/// <summary>
/// Gives the ability to receive push notifications.
/// </summary>
public interface IReceiver
{
    /// <summary>
    /// Gets a notification of an event.
    /// </summary>
    void OnReceive();
}
