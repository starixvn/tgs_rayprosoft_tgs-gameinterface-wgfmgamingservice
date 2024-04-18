namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public record PlayerKickCommand(
	Uri ApiUrl,
	string PartnerCode,
	string PartnerKey,
	string PartnerIV,
	string PlayerCode);
