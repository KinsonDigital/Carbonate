// <copyright file="IPullable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.OneWay;

/// <summary>
/// Gives the ability to pull data from a source using a messaging mechanism.
/// </summary>
/// <typeparam name="TOut">The type of data going back to the source.</typeparam>
public interface IPullable<out TOut>
{
    /// <summary>
    /// Requests to pull data from a source that matches the given <paramref name="respondId"/>.
    /// </summary>
    /// <param name="respondId">The ID of the response.</param>
    /// <returns>The data result going out.</returns>>
    TOut? Pull(Guid respondId);
}
