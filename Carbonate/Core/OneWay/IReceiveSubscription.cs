// <copyright file="IReceiveReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.OneWay;

/// <summary>
/// A reactor capable of standard functionality that can receive push notifications.
/// </summary>
/// <typeparam name="TIn">The type of data coming in.</typeparam>
public interface IReceiveSubscription<in TIn> : ISubscription, IReceiver<TIn>
{
}
