// <copyright file="IPushReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

/// <summary>
/// Defines a provider for push-based notification.
/// </summary>
public interface IPushReactable : IReactable<IReceiveReactor>, IPushable
{
}
