// <copyright file="IPullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using Core;

/// <summary>
/// Defines a provider for pull-based responses.
/// </summary>
public interface IPullReactable : IReactable<IRespondReactor>, IPullable
{
}
