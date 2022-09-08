using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.WGFmGamingService.Core.Models;

namespace Tgs.GameInterface.WGFmGamingService.Core.UnitTest.GameApiClientUnitTest;

[TestClass]
public class ShowBetListUnitTest
{
    [TestMethod]
    public async Task Normal_Process_BetDate()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var partnerKey = "partnerKey";
        var partnerIV = "partnerIV";
        var dateType = ShowBetListDateType.BetDate;
        var startTime = DateTimeOffset.Parse("2022-08-30 14:20:00+08:00");
        var endTime = DateTimeOffset.Parse("2022-08-30 14:25:00+08:00");

        var fakeAesCryptography = Substitute.For<IAesCryptography>();

        var dataEncrypt = Guid.NewGuid().ToString("N");

        var timeZone = TimeZoneInfo.CreateCustomTimeZone(id: "eastern time", baseUtcOffset: TimeSpan.FromHours(-4), "eastern time", "eastern time");
        var startTimeEastern = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), timeZone);
        var endTimeEastern = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), timeZone);

        var data = $"|{dateType.Description()}|{startTimeEastern:yyyy-MM-dd HH:mm:ss}|{endTimeEastern:yyyy-MM-dd HH:mm:ss}";

        _ = fakeAesCryptography.GetAesEncryptAsync(
            Arg.Is(data),
            Arg.Is(partnerKey),
            Arg.Is(partnerIV)).Returns(dataEncrypt);

        var url = new Uri($"{apiUrl}?m=api&c=index&a=ShowBetList&Hash={dataEncrypt}&PartnerCode={partnerCode}");

        var response = new ShowBetListResponse()
        {
            ReturnCode = 1,
            Total = "1",
            TotalActiveAmount = "0",
            TotalAmount = "50",
            TotalResult = "0",
            BetInfo = new ShowBetInfo[] {
                new ShowBetInfo() {
                    MemberId = "26673",
                    Username = "4ued4qr92gm0",
                    RecordDate = "2022-08-30 02:21:40",
                    Reference = "4083093533",
                    SportType = "篮球",
                    BetType = "让球",
                    MatchName = "FIBA欧洲篮球赛(在捷克/格鲁吉亚/意大利/德国)",
                    HomeName = "西班牙",
                    AwayName = "保加利亚",
                    BetContent = "FIBA欧洲篮球赛(在捷克/格鲁吉亚/意大利/德国)<br>西班牙18.5保加利亚 <br>西班牙 @&nbsp; 0.900",
                    BetAmount = "50.00",
                    BetStatus = "未结算",
                    Result = "0.00",
                    ActiveAmount = "0.00",
                    ResultScore = "",
                    SettleDate = "",
                    BetIp = "220.132.206.70",
                    InitData = new ShowBetInfoInitData()
                    {
                        FB = "2",
                        Type = "handicap",
                        Content = "home@0.90@-18.5",
                        Valid = "1",
                        LiveStatus = "1",
                        PlanNo = "1",
                        Part = "0",
                        Win = "0",
                        OddsType = "1",
                        MatchDate = "2022-09-01 07:30:00",
                        BetAmount = "50.00"
                    }
                }
            }
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

        var actual = await client.ShowBetListAsync(
            apiUrl,
            partnerCode,
            partnerKey,
            partnerIV,
            dateType,
            startTime,
            endTime
            ).ConfigureAwait(false);

        Assert.IsNotNull(actual);
    }

    [TestMethod]
    public async Task Normal_Process_SettleDate()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var partnerKey = "partnerKey";
        var partnerIV = "partnerIV";
        var dateType = ShowBetListDateType.SettleDate;
        var startTime = DateTimeOffset.Parse("2022-08-25 16:05:00+08:00");
        var endTime = DateTimeOffset.Parse("2022-08-25 16:10:00+08:00");

        var fakeAesCryptography = Substitute.For<IAesCryptography>();

        var dataEncrypt = Guid.NewGuid().ToString("N");
        var cuiabaTime = TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time");//(UTC-04:00) Cuiaba

        var startTimeUtc4 = TimeZoneInfo.ConvertTimeFromUtc(startTime.UtcDateTime, cuiabaTime);
        var endTimeUtc4 = TimeZoneInfo.ConvertTimeFromUtc(endTime.UtcDateTime, cuiabaTime);
        var data = $"|{dateType.Description()}|{startTimeUtc4:yyyy-MM-dd HH:mm:ss}|{endTimeUtc4:yyyy-MM-dd HH:mm:ss}";

        _ = fakeAesCryptography.GetAesEncryptAsync(
            Arg.Is(data),
            Arg.Is(partnerKey),
            Arg.Is(partnerIV)).Returns(dataEncrypt);

        var url = new Uri($"{apiUrl}?m=api&c=index&a=ShowBetList&Hash={dataEncrypt}&PartnerCode={partnerCode}");

        var response = new ShowBetListResponse()
        {
            ReturnCode = 1,
            Total = "3",
            TotalActiveAmount = "219.01",
            TotalAmount = "310",
            TotalResult = "120.64",
            BetInfo = new ShowBetInfo[] {
                new ShowBetInfo() {
                    MemberId = "35642",
                    Username = "4ufjeyz80v90",
                    RecordDate = "2022-08-25 02:46:20",
                    Reference = "3082562817",
                    SportType = "足球",
                    BetType = "(滚球)让球",
                    MatchName = "俱乐部友谊赛",
                    HomeName = "参孙卡法权瑟姆",
                    AwayName = "比达耶特拉维夫巴亚",
                    BetContent = "俱乐部友谊赛<br>参孙卡法权瑟姆0比达耶特拉维夫巴亚 (1:2)<br> 0:0 参孙卡法权瑟姆 @&nbsp; 0.880",
                    BetAmount = "50.00",
                    BetStatus = "已结算",
                    Result = "-49.63",
                    ActiveAmount = "50.00",
                    ResultScore = "0:1/1:2",
                    SettleDate = "2022-08-25 04:05:13",
                    BetIp = "123.125.127.189",
                    InitData = new ShowBetInfoInitData()
                    {
                        FB = "1",
                        Type = "live",
                        Content = "home@0.88@-0@0:0",
                        Valid = "1",
                        LiveStatus = "1",
                        PlanNo = "1",
                        Part = "0",
                        Win = "1",
                        OddsType = "1",
                        MatchDate = "2022-08-25 02:13:00",
                        BetAmount = "50.00"
                    }
                },
                new ShowBetInfo() {
                    MemberId = "35642",
                    Username = "4ufjeyz80v90",
                    RecordDate = "2022-08-25 02:44:52",
                    Reference = "8082591913",
                    SportType = "足球",
                    BetType = "(滚球)上半场让球",
                    MatchName = "俱乐部友谊赛",
                    HomeName = "参孙卡法权瑟姆",
                    AwayName = "比达耶特拉维夫巴亚",
                    BetContent = "俱乐部友谊赛<br>参孙卡法权瑟姆0比达耶特拉维夫巴亚 (上半场0:1)<br> 0:0 参孙卡法权瑟姆 @&nbsp; 0.940",
                    BetAmount = "50.00",
                    BetStatus = "<br><span style='color: red'>不接受</span>",
                    Result = "<span style='color: red'>不接受</span>",
                    ActiveAmount = "0.00",
                    ResultScore = "0:1/1:2",
                    SettleDate = "2022-08-25 04:05:13",
                    BetIp = "123.125.127.189",
                    InitData = new ShowBetInfoInitData()
                    {
                        FB = "1",
                        Type = "live_halftime",
                        Content = "home@0.94@-0@0:0",
                        Valid = "1",
                        LiveStatus = "1",
                        PlanNo = "1",
                        Part = "0",
                        Win = "1",
                        OddsType = "1",
                        MatchDate = "2022-08-25 02:13:00",
                        BetAmount = "50.00"
                    }
                },
                new ShowBetInfo() {
                    MemberId = "55017",
                    Username = "sup6666api_sakjune66",
                    RecordDate = "2022-08-25 00:38:47",
                    Reference = "6082524611",
                    SportType = "足球",
                    BetType = "让球",
                    MatchName = "俱乐部友谊赛",
                    HomeName = "参孙卡法权瑟姆",
                    AwayName = "比达耶特拉维夫巴亚",
                    BetContent = "俱乐部友谊赛<br>参孙卡法权瑟姆0 / 0.5比达耶特拉维夫巴亚 (1:2)<br>比达耶特拉维夫巴亚 @&nbsp; 0.650",
                    BetAmount = "260.00",
                    BetStatus = "已结算",
                    Result = "170.27",
                    ActiveAmount = "169.01",
                    ResultScore = "0:1/1:2",
                    SettleDate = "2022-08-25 04:05:13",
                    BetIp = "96.9.67.134",
                    InitData = new ShowBetInfoInitData()
                    {
                        FB = "1",
                        Type = "handicap",
                        Content = "away@0.65@0.25",
                        Valid = "1",
                        LiveStatus = "1",
                        PlanNo = "1",
                        Part = "0",
                        Win = "1",
                        OddsType = "1",
                        MatchDate = "2022-08-25 02:13:00",
                        BetAmount = "260.00"
                    }
                }
            }
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

        var actual = await client.ShowBetListAsync(
            apiUrl,
            partnerCode,
            partnerKey,
            partnerIV,
            dateType,
            startTime,
            endTime
            ).ConfigureAwait(false);

        Assert.IsNotNull(actual);
    }

    [TestMethod]
    public async Task Normal_Process_No_Data_Is_Null()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var partnerKey = "partnerKey";
        var partnerIV = "partnerIV";
        var dateType = ShowBetListDateType.SettleDate;
        var startTime = DateTimeOffset.Parse("2022-08-25 16:05:00+08:00");
        var endTime = DateTimeOffset.Parse("2022-08-25 16:10:00+08:00");

        var fakeAesCryptography = Substitute.For<IAesCryptography>();

        var dataEncrypt = Guid.NewGuid().ToString("N");
        var cuiabaTime = TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time");//(UTC-04:00) Cuiaba

        var startTimeUtc4 = TimeZoneInfo.ConvertTimeFromUtc(startTime.UtcDateTime, cuiabaTime);
        var endTimeUtc4 = TimeZoneInfo.ConvertTimeFromUtc(endTime.UtcDateTime, cuiabaTime);
        var data = $"|{dateType.Description()}|{startTimeUtc4:yyyy-MM-dd HH:mm:ss}|{endTimeUtc4:yyyy-MM-dd HH:mm:ss}";

        _ = fakeAesCryptography.GetAesEncryptAsync(
            Arg.Is(data),
            Arg.Is(partnerKey),
            Arg.Is(partnerIV)).Returns(dataEncrypt);

        var url = new Uri($"{apiUrl}?m=api&c=index&a=ShowBetList&Hash={dataEncrypt}&PartnerCode={partnerCode}");

        var response = new ShowBetListResponse()
        {
            ReturnCode = 1,
            Total = "0",
            TotalActiveAmount = "0",
            TotalAmount = "0",
            TotalResult = "0"
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

        var actual = await client.ShowBetListAsync(
            apiUrl,
            partnerCode,
            partnerKey,
            partnerIV,
            dateType,
            startTime,
            endTime
            ).ConfigureAwait(false);

        Assert.IsNotNull(actual);
    }

    [TestMethod]
    public async Task Normal_Process_No_Data()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var partnerKey = "partnerKey";
        var partnerIV = "partnerIV";
        var dateType = ShowBetListDateType.SettleDate;
        var startTime = DateTimeOffset.Parse("2022-08-25 16:05:00+08:00");
        var endTime = DateTimeOffset.Parse("2022-08-25 16:10:00+08:00");

        var fakeAesCryptography = Substitute.For<IAesCryptography>();

        var dataEncrypt = Guid.NewGuid().ToString("N");
        var cuiabaTime = TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time");//(UTC-04:00) Cuiaba

        var startTimeUtc4 = TimeZoneInfo.ConvertTimeFromUtc(startTime.UtcDateTime, cuiabaTime);
        var endTimeUtc4 = TimeZoneInfo.ConvertTimeFromUtc(endTime.UtcDateTime, cuiabaTime);
        var data = $"|{dateType.Description()}|{startTimeUtc4:yyyy-MM-dd HH:mm:ss}|{endTimeUtc4:yyyy-MM-dd HH:mm:ss}";

        _ = fakeAesCryptography.GetAesEncryptAsync(
            Arg.Is(data),
            Arg.Is(partnerKey),
            Arg.Is(partnerIV)).Returns(dataEncrypt);

        var url = new Uri($"{apiUrl}?m=api&c=index&a=ShowBetList&Hash={dataEncrypt}&PartnerCode={partnerCode}");

        var response = new ShowBetListResponse()
        {
            ReturnCode = 1,
            Total = "0",
            TotalActiveAmount = "0",
            TotalAmount = "0",
            TotalResult = "0",
            BetInfo = Array.Empty<ShowBetInfo>()
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

        var actual = await client.ShowBetListAsync(
            apiUrl,
            partnerCode,
            partnerKey,
            partnerIV,
            dateType,
            startTime,
            endTime
            ).ConfigureAwait(false);

        Assert.IsNotNull(actual);
    }
}
