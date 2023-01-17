// <copyright file="IRespondReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.BiDirectional;

/// <summary>
/// Provides a mechanism for receiving responses.
/// </summary>
/// <typeparam name="TDataIn">The data coming in.</typeparam>
/// <typeparam name="TDataOut">The data going out.</typeparam>
public interface IRespondReactor<in TDataIn, out TDataOut> : IReactor, IResponder<TDataIn, TDataOut>
{
}
