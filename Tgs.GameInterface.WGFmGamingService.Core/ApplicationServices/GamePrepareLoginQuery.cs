namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public record GamePrepareLoginQuery(
    Uri ApiUrl,
    string PartnerCode,
    string PartnerKey,
    string PartnerIV,
    string PlayerCode,
    string CurrencyCode);
