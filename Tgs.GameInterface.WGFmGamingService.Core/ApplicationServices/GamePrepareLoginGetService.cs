using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public class GamePrepareLoginGetService(
    IGameApiClient gameApiClient) : IGetService<GamePrepareLoginQuery, string>
{
    public ValueTask<string> GetAsync(
        GamePrepareLoginQuery query,
        CancellationToken cancellationToken = default)
        => gameApiClient
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
