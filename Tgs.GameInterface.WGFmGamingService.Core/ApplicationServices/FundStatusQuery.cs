namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public record FundStatusQuery(
	Uri ApiUrl,
	string PartnerCode,
	string PartnerKey,
	string PartnerIV,
	string OrderId);
