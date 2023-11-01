using NSubstitute;
using Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

namespace Tgs.GameInterface.WGFmGamingService.Core.UnitTest.ApplicationServicesUnitTest;

public class FundStatusGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var query = new FundStatusQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            Guid.NewGuid().ToString("N"));

        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        _ = fakeGameApiClient
                .AccessMoneyQueryAsync(
                    Arg.Is(query.ApiUrl),
                    Arg.Is(query.PartnerCode),
                    Arg.Is(query.PartnerKey),
                    Arg.Is(query.PartnerIV),
                    Arg.Is(query.OrderId))
                .Returns(true);

        var service = new FundStatusGetService(
            fakeGameApiClient);

        var actual = await service.GetAsync(query);

        Assert.True(actual);
    }
}
