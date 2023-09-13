// <copyright file="IRespondReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.TwoWay;

/// <summary>
/// Provides a mechanism for receiving responses.
/// </summary>
/// <typeparam name="TDataIn">The type of data coming in.</typeparam>
/// <typeparam name="TDataOut">The type of data going out.</typeparam>
public interface IRespondReactor<in TDataIn, out TDataOut> : IReactor, IResponder<TDataIn, TDataOut>
{
}
