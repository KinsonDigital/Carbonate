// <copyright file="IPushPullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable RedundantTypeDeclarationBody
namespace Carbonate.TwoWay;

using Core;
using Core.TwoWay;

/// <summary>
/// Defines a provider for pull-based responses.
/// </summary>
/// <typeparam name="TIn">The type of data coming from the source.</typeparam>
/// <typeparam name="TOut">The type of data going back to the source.</typeparam>
public interface IPushPullReactable<TIn, TOut> : IReactable<IReceiveRespondSubscription<TIn, TOut>>, IPushablePullable<TIn, TOut>
{
}
