﻿// <copyright file="IPushPullReactable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.TwoWay;

using Core;
using Core.TwoWay;

/// <summary>
/// Defines a provider for pull-based responses.
/// </summary>
/// <typeparam name="TDataIn">The type of data coming in.</typeparam>
/// <typeparam name="TDataOut">The type of data going out.</typeparam>
public interface IPushPullReactable<TDataIn, TDataOut> : IReactable<IRespondReactor<TDataIn, TDataOut>>, IPushablePullable<TDataIn, TDataOut>
{
}