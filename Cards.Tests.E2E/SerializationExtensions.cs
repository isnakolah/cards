using System.Text;
using System.Text.Json;

namespace Cards.Tests.E2E;

public static class SerializationExtensions
{
    private static JsonSerializerOptions DefaultJsonSerializerOptions => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static async Task<T?> DeserializeAsync<T>(this HttpContent response)
    {
        return  JsonSerializer.Deserialize<T>(
            await response.ReadAsStringAsync(),
            DefaultJsonSerializerOptions);
    }
    
    public static StringContent ToStringContent(this object body)
    {
        return new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
    }
}