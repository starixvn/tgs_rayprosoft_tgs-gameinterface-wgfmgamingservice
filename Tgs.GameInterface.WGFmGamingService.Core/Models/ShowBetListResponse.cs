using System.Text.Json.Serialization;

namespace Tgs.GameInterface.WGFmGamingService.Core.Models;

public record ShowBetListResponse
{
    [JsonPropertyName("ReturnCode")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int ReturnCode { get; init; }
    [JsonPropertyName("bet_info")]
    public IEnumerable<ShowBetInfo> BetInfo { get; set; } = default!;
    [JsonPropertyName("total")]
    public string Total { get; set; } = default!;
    [JsonPropertyName("total_amount")]
    public string TotalAmount { get; set; } = default!;
    [JsonPropertyName("total_result")]
    public string TotalResult { get; set; } = default!;
    [JsonPropertyName("total_active_amount")]
    public string TotalActiveAmount { get; set; } = default!;
}
