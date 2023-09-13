// <copyright file="IResponder.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.TwoWay;

/// <summary>
/// Gives the ability to respond to a pull request from an <see cref="IReactable{IResponder}"/>.
/// </summary>
/// <typeparam name="TDataIn">The type of data coming in.</typeparam>
/// <typeparam name="TDataOut">The type of data going out.</typeparam>
public interface IResponder<in TDataIn, out TDataOut>
{
    /// <summary>
    /// Returns a response.
    /// </summary>
    /// <param name="data">The data for the <see cref="IReactable{IResponder}"/>.</param>
    /// <returns>The data result going out.</returns>>
    TDataOut? OnRespond(TDataIn data);
}
