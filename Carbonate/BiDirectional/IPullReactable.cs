// <copyright file="IPullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.BiDirectional;

using Core;
using Core.BiDirectional;

/// <summary>
/// Defines a provider for pull-based responses.
/// </summary>
/// <typeparam name="TDataIn">The data coming in.</typeparam>
/// <typeparam name="TDataOut">The data going out.</typeparam>
public interface IPullReactable<TDataIn, TDataOut> : IReactable<IRespondReactor<TDataIn, TDataOut>>, IPullable<TDataIn, TDataOut>
{
}
