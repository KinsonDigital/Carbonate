// <copyright file="IPullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.UniDirectional;

using Core;
using Core.UniDirectional;

/// <summary>
/// Defines a provider for pull-based responses.
/// </summary>
/// <typeparam name="TDataOut">The data going out.</typeparam>
public interface IPullReactable<TDataOut> : IReactable<IRespondReactor<TDataOut>>, IPullable<TDataOut>
{
}
