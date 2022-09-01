using NSubstitute;
using Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

namespace Tgs.GameInterface.WGFmGamingService.Core.UnitTest.ApplicationServicesUnitTest;

[TestClass]
public class FundInCommandServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var command = new FundInCommand(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            "test",
            Guid.NewGuid().ToString("N"),
            1);

        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        _ = fakeGameApiClient
                .AccessMoneyWithdrawalAsync(
                    Arg.Is(command.ApiUrl),
                    Arg.Is(command.PartnerCode),
                    Arg.Is(command.PartnerKey),
                    Arg.Is(command.PartnerIV),
                    Arg.Is(command.PlayerCode),
                    Arg.Is(command.OrderId),
                    Arg.Is(command.Amount));

        var service = new FundInCommandService(
            fakeGameApiClient);

        await service
           .ExecuteAsync(
               command)
           .ConfigureAwait(false);
    }
}
