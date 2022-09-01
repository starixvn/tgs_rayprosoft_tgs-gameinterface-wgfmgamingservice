namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public record PlayerBalanceQuery(
    Uri ApiUrl,
    string PartnerCode,
    string PartnerKey,
    string PartnerIV,
    string PlayerCode);
