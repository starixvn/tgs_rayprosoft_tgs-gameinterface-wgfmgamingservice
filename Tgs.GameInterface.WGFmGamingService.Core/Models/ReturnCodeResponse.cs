using System.Text.Json.Serialization;

namespace Tgs.GameInterface.WGFmGamingService.Core.Models;

public record ReturnCodeResponse
{
    [JsonPropertyName("ReturnCode")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int ReturnCode { get; init; }
}
