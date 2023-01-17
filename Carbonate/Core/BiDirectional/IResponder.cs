// <copyright file="IResponder.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.BiDirectional;

/// <summary>
/// Gives the ability to respond to a pull request from an <see cref="IReactable{IResponder}"/>.
/// </summary>
/// <typeparam name="TDataIn">The data coming in.</typeparam>
/// <typeparam name="TDataOut">The data going out.</typeparam>
public interface IResponder<in TDataIn, out TDataOut>
{
    /// <summary>
    /// Returns a response.
    /// </summary>
    /// <param name="data">The data for the <see cref="IReactable{IResponder}"/>.</param>
    /// <returns>The response result.</returns>
    IResult<TDataOut>? OnRespond(TDataIn data);
}
