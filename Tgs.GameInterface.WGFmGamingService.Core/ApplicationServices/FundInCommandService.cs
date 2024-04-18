using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

internal class FundInCommandService(
	IGameApiClient gameApiClient) : ICommandService<FundInCommand>
{
	public ValueTask ExecuteAsync(
		FundInCommand command,
		CancellationToken cancellationToken = default)
		=> gameApiClient
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
