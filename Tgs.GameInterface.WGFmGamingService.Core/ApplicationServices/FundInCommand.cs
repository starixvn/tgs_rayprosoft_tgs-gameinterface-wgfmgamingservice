namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public record FundInCommand(
	Uri ApiUrl,
	string PartnerCode,
	string PartnerKey,
	string PartnerIV,
	string PlayerCode,
	string OrderId,
	decimal Amount);
