using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public class GamePrepareLoginGetService : IGetService<GamePrepareLoginQuery, string>
{
    private readonly IGameApiClient _gameApiClient;

    public GamePrepareLoginGetService(IGameApiClient gameApiClient)
    {
        _gameApiClient = gameApiClient ?? throw new ArgumentNullException(nameof(gameApiClient));
    }

    public ValueTask<string> GetAsync(
        GamePrepareLoginQuery query,
        CancellationToken cancellationToken = default)
        => _gameApiClient
            .PrepareLoginAsync(
                query.ApiUrl,
                query.PartnerCode,
                query.PartnerKey,
                query.PartnerIV,
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                query.PlayerCode,
                query.PlayerCode,
                query.CurrencyCode,
                cancellationToken);
}
