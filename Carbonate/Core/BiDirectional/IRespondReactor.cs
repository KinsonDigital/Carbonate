// <copyright file="IRespondReactor.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.BiDirectional;

/// <summary>
/// Provides a mechanism for receiving responses.
/// </summary>
/// <typeparam name="TDataIn">The type of data packaged in the incoming <see cref="IMessage{TDataIn}"/>.</typeparam>
/// <typeparam name="TDataOut">The type of data being returned in the response.</typeparam>
public interface IRespondReactor<in TDataIn, out TDataOut> : IReactor, IResponder<TDataIn, TDataOut>
{
}
