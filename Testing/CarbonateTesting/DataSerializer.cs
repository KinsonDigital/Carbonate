using System.Text.Json;
using Carbonate.Services;

namespace CarbonateTesting;

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
