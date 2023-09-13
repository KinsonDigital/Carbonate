// <copyright file="IPushReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.OneWay;

using Core;
using Core.OneWay;

/// <summary>
/// Defines a provider for push-based notifications.
/// </summary>
/// <typeparam name="TDataIn">The type of data coming in.</typeparam>
public interface IPushReactable<TDataIn> : IReactable<IReceiveReactor<TDataIn>>, IPushable<TDataIn>
{
}
