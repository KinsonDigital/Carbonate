// <copyright file="IResponder.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.OneWay;

/// <summary>
/// Gives the ability to respond to a pull request from an <see cref="IReactable{IResponder}"/>.
/// </summary>
/// <typeparam name="TOut">The type of data going back to the source.</typeparam>
public interface IResponder<out TOut>
{
    /// <summary>
    /// Returns a response.
    /// </summary>
    /// <returns>The data result going out.</returns>>
    TOut? OnRespond();
}
