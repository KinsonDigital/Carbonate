// <copyright file="IRespondSubscription.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.OneWay;

/// <summary>
/// Provides a mechanism for receiving responses.
/// </summary>
/// <typeparam name="TOut">The type of data going out.</typeparam>
public interface IRespondSubscription<out TOut> : ISubscription, IResponder<TOut>
{
}
