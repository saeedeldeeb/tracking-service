using System.Text.Json;

namespace TrackingService.Helpers;

public class JsonSerializerOptionsWrapper
{
    public JsonSerializerOptions? Options { get; }

    public JsonSerializerOptionsWrapper()
    {
        Options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            DefaultBufferSize = 10
        };
    }
}