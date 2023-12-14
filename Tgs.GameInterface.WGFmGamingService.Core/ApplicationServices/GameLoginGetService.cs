using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public class GameLoginGetService : IGetService<GameLoginQuery, Uri>
{
    private readonly IGameApiClient _gameApiClient;

    public GameLoginGetService(IGameApiClient gameApiClient)
    {
        _gameApiClient = gameApiClient ?? throw new ArgumentNullException(nameof(gameApiClient));
    }

    public ValueTask<Uri> GetAsync(GameLoginQuery query, CancellationToken cancellationToken = default)
        => _gameApiClient.LoginAsync(
            apiUrl: query.ApiUrl,
            partnerCode: query.PartnerCode,
            token: query.Token,
            lang: query.Lang,
            version: query.Version,
            IsPhone: query.IsPhone ? 1 : 0,
            cancellationToken: cancellationToken);
}
