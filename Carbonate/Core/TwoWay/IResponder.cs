// <copyright file="IResponder.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.TwoWay;

/// <summary>
/// Gives the ability to respond to a pull request from an <see cref="IReactable{IResponder}"/>.
/// </summary>
/// <typeparam name="TIn">The type of data coming from the source.</typeparam>
/// <typeparam name="TOut">The type of data going back to the source.</typeparam>
public interface IResponder<in TIn, out TOut>
{
    /// <summary>
    /// Returns a response.
    /// </summary>
    /// <param name="data">The data for the <see cref="IReactable{IResponder}"/>.</param>
    /// <returns>The data result going out.</returns>>
    TOut? OnRespond(TIn data);
}
