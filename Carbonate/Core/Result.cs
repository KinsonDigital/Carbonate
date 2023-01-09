// <copyright file="Result.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core;

using System.Text.Json;
using Services;

/// <summary>
/// Contains data that is returned as JSON data from an <see cref="IReactor"/>.
/// </summary>
/// <typeparam name="T">The type of data returned.</typeparam>
internal sealed class Result<T> : IResult<T>
{
    private readonly ISerializerService serializerService;
    private readonly string jsonData;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
    /// <param name="serializerService">The serializer used to deserialize the message.</param>
    /// <param name="jsonData">The JSON data.</param>
    public Result(ISerializerService serializerService, string jsonData)
    {
        this.serializerService = serializerService ?? throw new ArgumentNullException(
            nameof(serializerService),
            "The parameter must not be null.");

        if (string.IsNullOrEmpty(jsonData))
        {
            throw new ArgumentNullException(nameof(jsonData), "The string parameter must not be null or empty.");
        }

        this.jsonData = jsonData;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
#pragma warning disable CS8618
    internal Result()
    {
        // NOTE: Used for creating empty results from the result factory.
    }
#pragma warning restore CS8618

    /// <inheritdoc/>
    public bool IsEmpty { get; internal set; }

    /// <inheritdoc/>
    public T? GetValue(Action<Exception>? onError = null)
    {
        if (IsEmpty)
        {
            return default;
        }

        try
        {
            T? result = this.serializerService.Deserialize<T>(this.jsonData);

            if (result is null)
            {
                throw new JsonException("Issues with the JSON deserialization process.");
            }

            return result;
        }
        catch (Exception e)
        {
            IsEmpty = true;
            onError?.Invoke(e);
        }

        return default;
    }
}
