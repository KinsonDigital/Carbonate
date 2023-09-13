// <copyright file="IRespondSubscription.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.TwoWay;

/// <summary>
/// Provides a mechanism for receiving responses.
/// </summary>
/// <typeparam name="TIn">The type of data coming in.</typeparam>
/// <typeparam name="TOut">The type of data going out.</typeparam>
public interface IRespondSubscription<in TIn, out TOut> : ISubscription, IResponder<TIn, TOut>
{
}
