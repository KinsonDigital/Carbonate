// <copyright file="IPullable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Gives the ability to pull data from a source using a messaging mechanism.
/// </summary>
public interface IPullable
{
    /// <summary>
    /// Requests to pull data from a source that matches the given <paramref name="respondId"/>.
    /// </summary>
    /// <param name="respondId">The ID of the response.</param>
    /// <returns>The response result.</returns>
    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Public API.")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "Public API.")]
    IResult? Pull(Guid respondId);

    /// <summary>
    /// Requests to pull data from a source that matches the given <paramref name="respondId"/>,
    /// with the given additional <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The data to send to the responder.</param>
    /// <param name="respondId">The ID of the response.</param>
    /// <typeparam name="T">The type of data to send to the responder.</typeparam>
    /// <returns>The response result.</returns>
    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Public API.")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "Public API.")]
    IResult? Pull<T>(in T data, Guid respondId)
        where T : class;
}
