using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

internal class PlayerKickCommandService : ICommandService<PlayerKickCommand>
{
    private readonly IGameApiClient _gameApiClient;

    public PlayerKickCommandService(IGameApiClient gameApiClient)
    {
        _gameApiClient = gameApiClient ?? throw new ArgumentNullException(nameof(gameApiClient));
    }

    public ValueTask ExecuteAsync(
        PlayerKickCommand command,
        CancellationToken cancellationToken = default)
        => _gameApiClient
            .LogoutAsync(
                command.ApiUrl,
                command.PartnerCode,
                command.PartnerKey,
                command.PartnerIV,
                command.PlayerCode,
                cancellationToken);
}
