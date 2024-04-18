using System.Text.Json.Serialization;

namespace Tgs.GameInterface.WGFmGamingService.Core.Models;

public record LoginResponse
{
	[JsonPropertyName("ReturnCode")]
	[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
	public int ReturnCode { get; init; }
	[JsonPropertyName("LoginUrl")]
	public string LoginUrl { get; init; } = default!;
	[JsonPropertyName("MemberId")]
	public string MemberId { get; init; } = default!;
}
