using System.Text.Json.Serialization;

namespace Tgs.GameInterface.WGFmGamingService.Core.Models;

public record PrepareLoginResponse
{
	[JsonPropertyName("ReturnCode")]
	[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
	public int ReturnCode { get; init; }
	[JsonPropertyName("Token")]
	public string Token { get; init; } = default!;
}
