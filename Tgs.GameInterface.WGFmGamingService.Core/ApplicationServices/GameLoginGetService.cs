using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices
{
    public class GameLoginGetService : IGetService<GameLoginQuery, Uri>
    {
        private readonly IGameApiClient _gameApiClient;

        public GameLoginGetService(IGameApiClient gameApiClient)
        {
            _gameApiClient = gameApiClient ?? throw new ArgumentNullException(nameof(gameApiClient));
        }

        public ValueTask<Uri> GetAsync(
            GameLoginQuery query,
            CancellationToken cancellationToken = default)
            => _gameApiClient
                .LoginAsync(
                    query.ApiUrl,
                    query.PartnerCode,
                    query.Token,
                    query.Lang,
                    query.Version,
                    cancellationToken);
    }
}
