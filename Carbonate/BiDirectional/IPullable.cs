// <copyright file="IPullable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.BiDirectional;

using System.Diagnostics.CodeAnalysis;
using Core;

/// <summary>
/// Gives the ability to pull data from a source using a messaging mechanism.
/// </summary>
/// <typeparam name="TDataIn">The type of data packaged in the <see cref="IMessage{TDataIn}"/>.</typeparam>
/// <typeparam name="TDataOut">The type of data to be pulled.</typeparam>
public interface IPullable<TDataIn, out TDataOut>
{
    /// <summary>
    /// Requests to pull data from a source that matches the given <paramref name="respondId"/>,
    /// with the given additional <paramref name="msg"/>.
    /// </summary>
    /// <param name="msg">The data to send to the responder.</param>
    /// <param name="respondId">The ID of the response.</param>
    /// <typeparam name="TDataOut">The type of data to send to the responder.</typeparam>
    /// <returns>The response result.</returns>
    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Public API.")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "Public API.")]
    IResult<TDataOut> Pull(in IMessage<TDataIn> msg, Guid respondId);
}
