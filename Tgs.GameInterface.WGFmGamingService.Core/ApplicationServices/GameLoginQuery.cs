namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public record GameLoginQuery(
    Uri ApiUrl,
    string PartnerCode,
    string Token,
    string Lang);
