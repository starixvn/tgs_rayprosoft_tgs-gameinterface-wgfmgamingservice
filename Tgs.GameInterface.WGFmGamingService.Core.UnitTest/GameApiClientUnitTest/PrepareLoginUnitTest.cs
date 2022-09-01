using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.WGFmGamingService.Core.Models;

namespace Tgs.GameInterface.WGFmGamingService.Core.UnitTest.GameApiClientUnitTest;

[TestClass]
public class PrepareLoginUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var partnerKey = "partnerKey";
        var partnerIV = "partnerIV";
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var playerCode = "playerCode";
        var currencyCode = "RMB";

        var fakeAesCryptography = Substitute.For<IAesCryptography>();

        var dataEncrypt = Guid.NewGuid().ToString("N");
        var data = $"{timestamp}|{playerCode}|{playerCode}|{currencyCode}";

        _ = fakeAesCryptography.GetAesEncryptAsync(
            Arg.Is(data),
            Arg.Is(partnerKey),
            Arg.Is(partnerIV)).Returns(dataEncrypt);

        var url = new Uri($"{apiUrl}?m=api&c=index&a=PrepareLogin&Hash={dataEncrypt}&PartnerCode={partnerCode}");

        var response = new PrepareLoginResponse()
        {
            ReturnCode = 1,
            Token = Guid.NewGuid().ToString("N")
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

        var actual = await client.PrepareLoginAsync(
            apiUrl,
            partnerCode,
            partnerKey,
            partnerIV,
            timestamp,
            playerCode,
            playerCode,
            currencyCode
            ).ConfigureAwait(false);

        Assert.IsNotNull(actual);
    }
}
