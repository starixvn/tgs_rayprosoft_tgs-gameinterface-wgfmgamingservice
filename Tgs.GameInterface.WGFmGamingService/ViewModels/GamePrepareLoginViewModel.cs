using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.WGFmGamingService.ViewModels;

public record GamePrepareLoginViewModel
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
	public string PlayerCode { get; init; } = default!;
	[Required]
	public string CurrencyCode { get; init; } = default!;
}
