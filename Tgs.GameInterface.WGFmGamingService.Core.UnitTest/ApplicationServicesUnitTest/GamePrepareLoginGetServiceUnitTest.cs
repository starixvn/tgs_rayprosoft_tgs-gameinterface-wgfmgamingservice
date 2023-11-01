using NSubstitute;
using Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

namespace Tgs.GameInterface.WGFmGamingService.Core.UnitTest.ApplicationServicesUnitTest;

public class GamePrepareLoginGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var query = new GamePrepareLoginQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            "test",
            "RMB");

        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var token = Guid.NewGuid().ToString("N");
        _ = fakeGameApiClient
                .PrepareLoginAsync(
                    Arg.Is(query.ApiUrl),
                    Arg.Is(query.PartnerCode),
                    Arg.Is(query.PartnerKey),
                    Arg.Is(query.PartnerIV),
                    Arg.Any<long>(),
                    Arg.Is(query.PlayerCode),
                    Arg.Is(query.PlayerCode),
                    Arg.Is(query.CurrencyCode))
                .Returns(token);

        var service = new GamePrepareLoginGetService(
            fakeGameApiClient);

        var actual = await service.GetAsync(query);

        Assert.NotNull(actual);
        Assert.Equal(token, actual);
    }
}
