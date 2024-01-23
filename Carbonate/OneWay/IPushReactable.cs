// <copyright file="IPushReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable RedundantTypeDeclarationBody
namespace Carbonate.OneWay;

using Core;
using Core.OneWay;

/// <summary>
/// Defines a provider for push-based notifications.
/// </summary>
/// <typeparam name="TIn">The type of data coming from the source.</typeparam>
public interface IPushReactable<TIn> : IReactable<IReceiveSubscription<TIn>>, IPushable<TIn>
{
}
