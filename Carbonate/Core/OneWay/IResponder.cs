// <copyright file="IResponder.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core.OneWay;

/// <summary>
/// Gives the ability to respond to a pull request from an <see cref="IReactable{IResponder}"/>.
/// </summary>
/// <typeparam name="TDataOut">The type of data going out.</typeparam>
public interface IResponder<out TDataOut>
{
    /// <summary>
    /// Returns a response.
    /// </summary>
    /// <returns>The data result going out.</returns>>
    TDataOut? OnRespond();
}
