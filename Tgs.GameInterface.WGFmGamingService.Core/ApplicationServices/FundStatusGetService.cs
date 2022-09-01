using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices
{
    public class FundStatusGetService : IGetService<FundStatusQuery, bool>
    {
        private readonly IGameApiClient _gameApiClient;

        public FundStatusGetService(IGameApiClient gameApiClient)
        {
            _gameApiClient = gameApiClient ?? throw new ArgumentNullException(nameof(gameApiClient));
        }

        public ValueTask<bool> GetAsync(
            FundStatusQuery query,
            CancellationToken cancellationToken = default)
            => _gameApiClient
                .AccessMoneyQueryAsync(
                    query.ApiUrl,
                    query.PartnerCode,
                    query.PartnerKey,
                    query.PartnerIV,
                    query.OrderId,
                    cancellationToken);
    }
}
