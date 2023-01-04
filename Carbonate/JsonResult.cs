// <copyright file="JsonResult.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate;

using System.Text.Json;
using Services;

/// <summary>
/// Contains data that is returned as JSON data from a <see cref="IRespondReactor"/> and <see cref="PullReactable"/>.
/// </summary>
internal sealed class JsonResult : IResult
{
    private readonly ISerializerService serializerService;
    private readonly string jsonData;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonResult"/> class.
    /// </summary>
    /// <param name="serializerService">The serializer used to deserialize the message.</param>
    /// <param name="jsonData">The JSON data.</param>
    public JsonResult(ISerializerService serializerService, string jsonData)
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
    public T? GetValue<T>(Action<Exception>? onError = null)
        where T : class
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

        return null;
    }
}
