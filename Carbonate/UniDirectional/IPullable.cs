// <copyright file="IPullable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.UniDirectional;

using System.Diagnostics.CodeAnalysis;
using Core;

/// <summary>
/// Gives the ability to pull data from a source using a messaging mechanism.
/// </summary>
/// <typeparam name="TDataOut">The data going out.</typeparam>
public interface IPullable<out TDataOut>
{
    /// <summary>
    /// Requests to pull data from a source that matches the given <paramref name="respondId"/>.
    /// </summary>
    /// <param name="respondId">The ID of the response.</param>
    /// <returns>The response result.</returns>
    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Public API.")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "Public API.")]
    IResult<TDataOut> Pull(Guid respondId);
}
