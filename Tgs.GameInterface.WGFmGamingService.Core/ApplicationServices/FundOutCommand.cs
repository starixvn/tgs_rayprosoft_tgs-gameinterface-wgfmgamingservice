namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public record FundOutCommand(
	Uri ApiUrl,
	string PartnerCode,
	string PartnerKey,
	string PartnerIV,
	string PlayerCode,
	string OrderId,
	decimal Amount);
