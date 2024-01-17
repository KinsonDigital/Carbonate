// <copyright file="IReceiveRespondSubscription.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable RedundantTypeDeclarationBody
namespace Carbonate.Core.TwoWay;

/// <summary>
/// Provides a mechanism for receiving notifications and responding with data.
/// </summary>
/// <typeparam name="TIn">The type of data coming from the source.</typeparam>
/// <typeparam name="TOut">The type of data going back to the source.</typeparam>
public interface IReceiveRespondSubscription<in TIn, out TOut> : ISubscription, IResponder<TIn, TOut>
{
}
