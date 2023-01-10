// <copyright file="ResultFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Diagnostics.CodeAnalysis;
using Core;
using Services;

/// <summary>
/// Used to create <see cref="IResult"/> objects for sending results back to a <see cref="IReactable{IRespondReactor}"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ResultFactory
{
    private static readonly ISerializerService SerializerService = new JsonSerializerService();

    /// <summary>
    /// Creates an <see cref="IResult"/> object that wraps the given <paramref name="data"/> for responding
    /// to a request from a <see cref="PullReactable"/>.
    /// </summary>
    /// <param name="data">The data to send out with the result.</param>
    /// <typeparam name="T">The type of data to send.</typeparam>
    /// <returns>The result that contains the data.</returns>
    /// <returns>
    ///     The data can be retrieved from the result by using <see cref="IResult"/>.<see cref="IResult.GetValue{T}"/>.
    /// </returns>
    public static IResult CreateResult<T>(T data)
    {
        var jsonData = SerializerService.Serialize(data);
        return new JsonResult(SerializerService, jsonData);
    }
}
