using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public class GameLoginGetService(
	IGameApiClient gameApiClient) : IGetService<GameLoginQuery, Uri>
{
	public ValueTask<Uri> GetAsync(GameLoginQuery query, CancellationToken cancellationToken = default)
		=> gameApiClient.LoginAsync(
			apiUrl: query.ApiUrl,
			partnerCode: query.PartnerCode,
			token: query.Token,
			lang: query.Lang,
			version: query.Version,
			IsPhone: query.IsPhone ? 1 : 0,
			cancellationToken: cancellationToken);
}
