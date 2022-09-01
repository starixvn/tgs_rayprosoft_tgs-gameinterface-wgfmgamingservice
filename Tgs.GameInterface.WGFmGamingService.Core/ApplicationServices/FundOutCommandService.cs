using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

internal class FundOutCommandService : ICommandService<FundOutCommand>
{
    private readonly IGameApiClient _gameApiClient;

    public FundOutCommandService(IGameApiClient gameApiClient)
    {
        _gameApiClient = gameApiClient ?? throw new ArgumentNullException(nameof(gameApiClient));
    }

    public ValueTask ExecuteAsync(
        FundOutCommand command,
        CancellationToken cancellationToken = default)
        => _gameApiClient
            .AccessMoneyDepositAsync(
                command.ApiUrl,
                command.PartnerCode,
                command.PartnerKey,
                command.PartnerIV,
                command.PlayerCode,
                command.OrderId,
                command.Amount,
                cancellationToken);
}
