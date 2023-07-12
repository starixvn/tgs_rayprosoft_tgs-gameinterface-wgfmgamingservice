using NSubstitute;
using Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

namespace Tgs.GameInterface.WGFmGamingService.Core.UnitTest.ApplicationServicesUnitTest;

public class FundOutCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var command = new FundOutCommand(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            "test",
            Guid.NewGuid().ToString("N"),
            1);

        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        _ = fakeGameApiClient
                .AccessMoneyDepositAsync(
                    Arg.Is(command.ApiUrl),
                    Arg.Is(command.PartnerCode),
                    Arg.Is(command.PartnerKey),
                    Arg.Is(command.PartnerIV),
                    Arg.Is(command.PlayerCode),
                    Arg.Is(command.OrderId),
                    Arg.Is(command.Amount));

        var service = new FundOutCommandService(
            fakeGameApiClient);

        await service
           .ExecuteAsync(
               command)
           .ConfigureAwait(false);
    }
}
