// <copyright file="JsonSerializer.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace Carbonate.Services;

using System.Text.Json;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Performs JSON services.
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed class JsonSerializer : ISerializer
{
    /// <inheritdoc/>
    public string Serialize<T>(T? value)
    {
        var options = new JsonSerializerOptions();
        options.WriteIndented = true;

        return System.Text.Json.JsonSerializer.Serialize(value, options);
    }

    /// <inheritdoc/>
    public T? Deserialize<T>(string value) => System.Text.Json.JsonSerializer.Deserialize<T>(value);
}
