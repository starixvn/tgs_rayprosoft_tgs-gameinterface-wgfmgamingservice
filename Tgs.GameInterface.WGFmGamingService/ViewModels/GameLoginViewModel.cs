using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.WGFmGamingService.ViewModels;

public record GameLoginViewModel
{
	[Required]
	public Uri ApiUrl { get; init; } = default!;
	[Required]
	public string PartnerCode { get; init; } = default!;
	[Required]
	public string Token { get; init; } = default!;
	[Required]
	public string Lang { get; init; } = default!;

	public int Version { get; init; } = 1;
	[Required]
	public bool IsPhone { get; init; } = false;
}
