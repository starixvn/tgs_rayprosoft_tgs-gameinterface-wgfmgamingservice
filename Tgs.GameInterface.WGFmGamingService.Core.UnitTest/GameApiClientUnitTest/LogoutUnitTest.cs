using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.WGFmGamingService.Core.Models;

namespace Tgs.GameInterface.WGFmGamingService.Core.UnitTest.GameApiClientUnitTest;

[TestClass]
public class LogoutUnitTest
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
        var data = playerCode;

        _ = fakeAesCryptography.GetAesEncryptAsync(
            Arg.Is(data),
            Arg.Is(partnerKey),
            Arg.Is(partnerIV)).Returns(dataEncrypt);

        var url = new Uri($"{apiUrl}?m=api&c=index&a=Logout&Hash={dataEncrypt}&PartnerCode={partnerCode}");

        var response = new ReturnCodeResponse()
        {
            ReturnCode = 1
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

        await client
            .LogoutAsync(
                apiUrl,
                partnerCode,
                partnerKey,
                partnerIV,
                playerCode)
            .ConfigureAwait(false);
    }
}
