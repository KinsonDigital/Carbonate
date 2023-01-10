// <copyright file="ResultFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Diagnostics.CodeAnalysis;
using BiDirectional;
using Core;
using Services;
using UniDirectional;

/// <summary>
/// Creates <see cref="IResult{TDataOut}"/> objects for sending results back to an <see cref="IReactable{IRespondReactor}"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ResultFactory
{
    private static readonly ISerializerService SerializerService = new JsonSerializerService();

    /// <summary>
    /// Creates an <see cref="IResult{TDataOut}"/> object that wraps the given <paramref name="data"/> for responding
    /// to a request from an <see cref="IPullReactable{TDataIn,TDataOut}"/> or an <see cref="IPullReactable{TDataOut}"/>.
    /// </summary>
    /// <param name="data">The data to send out with the result.</param>
    /// <typeparam name="T">The type of data to package.</typeparam>
    /// <returns>The result that contains the data.</returns>
    /// <remarks>
    ///     The data can be retrieved from the result by using <see cref="IResult{TDataOut}"/>.<see cref="IResult{TDataOut}.GetValue"/>.
    /// </remarks>
    public static IResult<T> CreateResult<T>(T data)
    {
        var jsonData = SerializerService.Serialize(data);
        return new Result<T>(SerializerService, jsonData);
    }

    /// <summary>
    /// Creates an empty <see cref="IResult{TDataOut}"/> object.  This is what will get returned if
    /// there is not data inside or if something has gone wrong.
    /// </summary>
    /// <typeparam name="T">The type of data to package.</typeparam>
    /// <returns>An empty result that does not contain data.</returns>
    /// <remarks>
    ///     Will return null if the result is empty.
    /// </remarks>
    public static IResult<T> CreateEmptyResult<T>() => new Result<T> { IsEmpty = true };
}
