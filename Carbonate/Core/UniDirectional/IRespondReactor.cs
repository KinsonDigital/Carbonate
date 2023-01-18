// <copyright file="IRespondReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.UniDirectional;

/// <summary>
/// Provides a mechanism for receiving responses.
/// </summary>
/// <typeparam name="TDataOut">The type of data going out.</typeparam>
public interface IRespondReactor<out TDataOut> : IReactor, IResponder<TDataOut>
{
}
