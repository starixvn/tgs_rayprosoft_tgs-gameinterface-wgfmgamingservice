using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public class PlayerBalanceGetService(
	IGameApiClient gameApiClient) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
	public async ValueTask<PlayerBalanceResult> GetAsync(
		PlayerBalanceQuery query,
		CancellationToken cancellationToken = default)
	{
		var (IsPlayer, Balance) = await gameApiClient
				.GetBalanceAsync(
					query.ApiUrl,
					query.PartnerCode,
					query.PartnerKey,
					query.PartnerIV,
					query.PlayerCode,
					cancellationToken)
				.ConfigureAwait(false);

		return new PlayerBalanceResult(
			IsPlayer,
			Balance);
	}
}
