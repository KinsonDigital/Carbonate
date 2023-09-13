﻿// <copyright file="IRespondReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.TwoWay;

/// <summary>
/// Provides a mechanism for receiving responses.
/// </summary>
/// <typeparam name="TIn">The type of data coming in.</typeparam>
/// <typeparam name="TOut">The type of data going out.</typeparam>
public interface IRespondReactor<in TIn, out TOut> : IReactor, IResponder<TIn, TOut>
{
}
