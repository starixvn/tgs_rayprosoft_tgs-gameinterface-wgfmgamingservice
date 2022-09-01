namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public record PlayerKickLineCommand(
    Uri ApiUrl,
    string PartnerCode,
    string PartnerKey,
    string PartnerIV,
    string PlayerCode);
