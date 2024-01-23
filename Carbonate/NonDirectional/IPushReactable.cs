// <copyright file="IPushReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable RedundantTypeDeclarationBody
namespace Carbonate.NonDirectional;

using Core;
using Core.NonDirectional;

/// <summary>
/// Defines a provider for push-based notifications.
/// </summary>
public interface IPushReactable : IReactable<IReceiveSubscription>, IPushable
{
}
