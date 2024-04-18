using System.Text.Json.Serialization;

namespace Tgs.GameInterface.WGFmGamingService.Core.Models;

public record GetBalanceResponse
{
	[JsonPropertyName("ReturnCode")]
	[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
	public int ReturnCode { get; init; }
	[JsonPropertyName("Balance")]
	public string Balance { get; init; } = default!;
	[JsonPropertyName("Currency")]
	public string Currency { get; init; } = default!;
}
