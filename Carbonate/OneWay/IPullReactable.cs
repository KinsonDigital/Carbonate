// <copyright file="IPullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable RedundantTypeDeclarationBody
namespace Carbonate.OneWay;

using Core;
using Core.OneWay;

/// <summary>
/// Defines a provider for pull-based responses.
/// </summary>
/// <typeparam name="TOut">The type of data going back to the source.</typeparam>
public interface IPullReactable<TOut> : IReactable<IRespondSubscription<TOut>>, IPullable<TOut>
{
}
