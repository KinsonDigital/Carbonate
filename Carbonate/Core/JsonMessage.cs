// <copyright file="JsonMessage.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Core;

using System.Text.Json;
using Services;

/// <summary>
/// A message that contains JSON data as a message that can be deserialized into an object.
/// </summary>
/// <typeparam name="T">The type of data in the message.</typeparam>
internal sealed class JsonMessage<T> : IMessage<T>
{
    private readonly ISerializerService serializerService;
    private readonly string jsonData;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonMessage{T}"/> class.
    /// </summary>
    /// <param name="serializerService">The serializer used to deserialize the message.</param>
    /// <param name="jsonData">The JSON data.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="serializerService"/> is null.</exception>
    public JsonMessage(ISerializerService serializerService, string jsonData)
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

    /// <inheritdoc/>
    public T? GetData(Action<Exception>? onError = null)
    {
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
            onError?.Invoke(e);
        }

        return default;
    }
}
