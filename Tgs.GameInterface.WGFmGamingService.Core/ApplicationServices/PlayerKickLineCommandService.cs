using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

internal class PlayerKickLineCommandService : ICommandService<PlayerKickLineCommand>
{
    private readonly IGameApiClient _gameApiClient;

    public PlayerKickLineCommandService(IGameApiClient gameApiClient)
    {
        _gameApiClient = gameApiClient ?? throw new ArgumentNullException(nameof(gameApiClient));
    }

    public ValueTask ExecuteAsync(
        PlayerKickLineCommand command,
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
