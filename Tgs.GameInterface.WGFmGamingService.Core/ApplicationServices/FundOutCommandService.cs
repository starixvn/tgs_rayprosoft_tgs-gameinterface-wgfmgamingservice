using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

internal class FundOutCommandService(
    IGameApiClient gameApiClient) : ICommandService<FundOutCommand>
{
    public ValueTask ExecuteAsync(
        FundOutCommand command,
        CancellationToken cancellationToken = default)
        => gameApiClient
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
