using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public class PlayerBalanceGetService : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    private readonly IGameApiClient _gameApiClient;

    public PlayerBalanceGetService(IGameApiClient gameApiClient)
    {
        _gameApiClient = gameApiClient ?? throw new ArgumentNullException(nameof(gameApiClient));
    }

    public async ValueTask<PlayerBalanceResult> GetAsync(
        PlayerBalanceQuery query,
        CancellationToken cancellationToken = default)
    {
        var (IsPlayer, Balance) = await _gameApiClient
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
