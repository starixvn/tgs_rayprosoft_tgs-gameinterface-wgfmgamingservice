namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public record GameHistoryQuery(
	Uri ApiUrl,
	string PartnerCode,
	string PartnerKey,
	string PartnerIV,
	bool IsSettle,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime);
