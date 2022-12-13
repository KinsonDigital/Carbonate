// <copyright file="DataSerializer.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace CarbonateTesting;

using System.Text.Json;
using Carbonate.Services;

public class DataSerializer : ISerializer
{
    public string Serialize<T>(T? value)
    {
        return JsonSerializer.Serialize(value);
    }

    public T? Deserialize<T>(string value)
    {
        return JsonSerializer.Deserialize<T>(value);
    }
}
