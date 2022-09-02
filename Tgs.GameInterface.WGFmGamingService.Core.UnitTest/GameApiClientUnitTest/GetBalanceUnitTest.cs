using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.WGFmGamingService.Core.Models;

namespace Tgs.GameInterface.WGFmGamingService.Core.UnitTest.GameApiClientUnitTest;

[TestClass]
public class GetBalanceUnitTest
{
    [TestMethod]
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

        var actual = await client.GetBalanceAsync(
            apiUrl,
            partnerCode,
            partnerKey,
            partnerIV,
            playerCode
            ).ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.IsPlayer);
        Assert.AreEqual(decimal.Parse(response.Balance), actual.Balance);
    }

    [TestMethod]
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

        var actual = await client.GetBalanceAsync(
            apiUrl,
            partnerCode,
            partnerKey,
            partnerIV,
            playerCode
            ).ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsFalse(actual.IsPlayer);
    }
}
