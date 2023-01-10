// <copyright file="IPullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.BiDirectional;

using Core;
using Core.BiDirectional;

/// <summary>
/// Defines a provider for pull-based responses.
/// </summary>
/// <typeparam name="TDataIn">The type of data packaged in the outgoing <see cref="IMessage{TDataIn}"/>.</typeparam>
/// <typeparam name="TDataOut">The type of data to pull.</typeparam>
public interface IPullReactable<TDataIn, TDataOut> : IReactable<IRespondReactor<TDataIn, TDataOut>>, IPullable<TDataIn, TDataOut>
{
}
