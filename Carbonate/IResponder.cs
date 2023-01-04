// <copyright file="IResponder.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

/// <summary>
/// Gives the ability to respond to a pull request from an <see cref="IReactable{IResponder}"/>.
/// </summary>
public interface IResponder
{
    /// <summary>
    /// Returns a response.
    /// </summary>
    /// <returns>The response result.</returns>
    IResult? OnRespond();

    /// <summary>
    /// Returns a response.
    /// </summary>
    /// <param name="message">The message for the <see cref="IReactable{IResponder}"/>.</param>
    /// <returns>The response result.</returns>
    IResult? OnRespond(IMessage message);
}
