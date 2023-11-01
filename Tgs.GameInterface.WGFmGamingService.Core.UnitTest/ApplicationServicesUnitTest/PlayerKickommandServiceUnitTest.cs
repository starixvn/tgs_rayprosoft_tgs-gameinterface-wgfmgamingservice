using NSubstitute;
using Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

namespace Tgs.GameInterface.WGFmGamingService.Core.UnitTest.ApplicationServicesUnitTest;

public class PlayerKickommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var command = new PlayerKickCommand(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            "test");

        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        _ = fakeGameApiClient
                .LogoutAsync(
                    Arg.Is(command.ApiUrl),
                    Arg.Is(command.PartnerCode),
                    Arg.Is(command.PartnerKey),
                    Arg.Is(command.PartnerIV),
                    Arg.Is(command.PlayerCode));

        var service = new PlayerKickCommandService(
            fakeGameApiClient);

        await service.ExecuteAsync(command);
    }
}
