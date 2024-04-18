using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

internal class PlayerKickCommandService(
	IGameApiClient gameApiClient) : ICommandService<PlayerKickCommand>
{
	public ValueTask ExecuteAsync(
		PlayerKickCommand command,
		CancellationToken cancellationToken = default)
		=> gameApiClient
			.LogoutAsync(
				command.ApiUrl,
				command.PartnerCode,
				command.PartnerKey,
				command.PartnerIV,
				command.PlayerCode,
				cancellationToken);
}
