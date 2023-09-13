// <copyright file="IPullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.OneWay;

using Core;
using Core.OneWay;

/// <summary>
/// Defines a provider for pull-based responses.
/// </summary>
/// <typeparam name="TDataOut">The type of data going out.</typeparam>
public interface IPullReactable<TDataOut> : IReactable<IRespondReactor<TDataOut>>, IPullable<TDataOut>
{
}
