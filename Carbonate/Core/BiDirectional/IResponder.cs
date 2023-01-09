// <copyright file="IResponder.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.BiDirectional;

/// <summary>
/// Gives the ability to respond to a pull request from an <see cref="IReactable{IResponder}"/>.
/// </summary>
/// <typeparam name="TDataIn">The data packaged in the <see cref="IMessage{TDataIn}"/>.</typeparam>
/// <typeparam name="TDataOut">The type of data being returned in the <see cref="IResult{TDataOut}"/>.</typeparam>
public interface IResponder<in TDataIn, out TDataOut>
{
    /// <summary>
    /// Returns a response.
    /// </summary>
    /// <param name="message">The message for the <see cref="IReactable{IResponder}"/>.</param>
    /// <returns>The response result.</returns>
    IResult<TDataOut>? OnRespond(IMessage<TDataIn> message);
}
