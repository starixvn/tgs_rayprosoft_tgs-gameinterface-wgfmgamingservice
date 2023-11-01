using NSubstitute;
using Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

namespace Tgs.GameInterface.WGFmGamingService.Core.UnitTest.ApplicationServicesUnitTest;

public class GameLoginGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var query = new GameLoginQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            Guid.NewGuid().ToString("N"),
            "0");

        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        _ = fakeGameApiClient
                .LoginAsync(
                    Arg.Is(query.ApiUrl),
                    Arg.Is(query.PartnerCode),
                    Arg.Is(query.Token),
                    Arg.Is(query.Lang))
                .Returns(new Uri("http://localhost:8088/"));

        var service = new GameLoginGetService(
            fakeGameApiClient);

        var actual = await service.GetAsync(query);

        Assert.NotNull(actual);
        Assert.Equal(new Uri("http://localhost:8088/"), actual);
    }
}
