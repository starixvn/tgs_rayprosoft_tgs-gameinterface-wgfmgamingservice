using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

public class FundStatusGetService(
    IGameApiClient gameApiClient) : IGetService<FundStatusQuery, bool>
{
    public ValueTask<bool> GetAsync(
        FundStatusQuery query,
        CancellationToken cancellationToken = default)
        => gameApiClient
            .AccessMoneyQueryAsync(
                query.ApiUrl,
                query.PartnerCode,
                query.PartnerKey,
                query.PartnerIV,
                query.OrderId,
                cancellationToken);
}
