using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

internal class FundInCommandService : ICommandService<FundInCommand>
{
    private readonly IGameApiClient _gameApiClient;

    public FundInCommandService(IGameApiClient gameApiClient)
    {
        _gameApiClient = gameApiClient ?? throw new ArgumentNullException(nameof(gameApiClient));
    }

    public ValueTask ExecuteAsync(
        FundInCommand command,
        CancellationToken cancellationToken = default)
        => _gameApiClient
            .AccessMoneyWithdrawalAsync(
                command.ApiUrl,
                command.PartnerCode,
                command.PartnerKey,
                command.PartnerIV,
                command.PlayerCode,
                command.OrderId,
                command.Amount,
                cancellationToken);
}
