using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.WGFmGamingService.Core.Models;

namespace Tgs.GameInterface.WGFmGamingService.Core.UnitTest.GameApiClientUnitTest;

[TestClass]
public class LoginUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var token = Guid.NewGuid().ToString("N");
        var lang = "0";

        var fakeAesCryptography = Substitute.For<IAesCryptography>();

        var url = new Uri($"{apiUrl}?m=api&c=index&a=Login&Token={WebUtility.UrlEncode(token)}&PartnerCode={partnerCode}&Lang={lang}&Version=2");

        var response = new LoginResponse()
        {
            ReturnCode = 1,
            LoginUrl = "http://localhost:8088/"
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

        var actual = await client.LoginAsync(
            apiUrl,
            partnerCode,
            token,
            lang).ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(new Uri("http://localhost:8088/"), actual);
    }
}
