using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.WGFmGamingService.ViewModels;

public record GameHistoryViewModel
{
    [Required]
    public Uri ApiUrl { get; init; } = default!;
    [Required]
    public string PartnerCode { get; init; } = default!;
    [Required]
    public string PartnerKey { get; init; } = default!;
    [Required]
    public string PartnerIV { get; init; } = default!;
    [Required]
    public bool IsSettle { get; init; } = default!;
    [Required]
    public DateTimeOffset StartTime { get; init; } = default!;
    [Required]
    public DateTimeOffset EndTime { get; init; } = default!;
}
