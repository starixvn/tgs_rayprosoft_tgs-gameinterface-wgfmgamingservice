using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.WGFmGamingService.Core.Models;

namespace Tgs.GameInterface.WGFmGamingService.Core.UnitTest.GameApiClientUnitTest;

public class GetBalanceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var partnerKey = "partnerKey";
        var partnerIV = "partnerIV";
        var playerCode = "playerCode";

        var fakeAesCryptography = Substitute.For<IAesCryptography>();
        var dataEncrypt = Guid.NewGuid().ToString("N");

        _ = fakeAesCryptography.GetAesEncryptAsync(
            Arg.Is(playerCode),
            Arg.Is(partnerKey),
            Arg.Is(partnerIV)).Returns(dataEncrypt);

        var url = new Uri(apiUrl, $"index.php?m=api&c=index&a=GetBalance&Hash={dataEncrypt}&PartnerCode={partnerCode}");

        var response = new GetBalanceResponse()
        {
            ReturnCode = 1,
            Balance = "0.00",
            Currency = "RMB"
        };
        var content = JsonSerializer.Serialize(response);
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Get, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient,
            fakeAesCryptography);

        var (IsPlayer, Balance) = await client.GetBalanceAsync(
            apiUrl,
            partnerCode,
            partnerKey,
            partnerIV,
            playerCode
            ).ConfigureAwait(false);

        Assert.True(IsPlayer);
        Assert.Equal(decimal.Parse(response.Balance), Balance);
    }

    [Fact]
    public async Task Player_Does_Not_Exist()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var partnerKey = "partnerKey";
        var partnerIV = "partnerIV";
        var playerCode = "playerCode";

        var fakeAesCryptography = Substitute.For<IAesCryptography>();
        var dataEncrypt = Guid.NewGuid().ToString("N");

        _ = fakeAesCryptography.GetAesEncryptAsync(
            Arg.Is(playerCode),
            Arg.Is(partnerKey),
            Arg.Is(partnerIV)).Returns(dataEncrypt);

        var url = new Uri(apiUrl, $"index.php?m=api&c=index&a=GetBalance&Hash={dataEncrypt}&PartnerCode={partnerCode}");

        var response = new GetBalanceResponse()
        {
            ReturnCode = 602
        };
        var content = JsonSerializer.Serialize(response);
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Get, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient,
            fakeAesCryptography);

        var (IsPlayer, _) = await client.GetBalanceAsync(
            apiUrl,
            partnerCode,
            partnerKey,
            partnerIV,
            playerCode
            ).ConfigureAwait(false);

        Assert.False(IsPlayer);
    }
}
