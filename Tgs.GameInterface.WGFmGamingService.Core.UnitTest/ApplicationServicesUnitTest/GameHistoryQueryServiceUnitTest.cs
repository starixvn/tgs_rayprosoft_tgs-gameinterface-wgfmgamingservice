using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;
using Tgs.GameInterface.WGFmGamingService.Core.Models;

namespace Tgs.GameInterface.WGFmGamingService.Core.UnitTest.ApplicationServicesUnitTest;

[TestClass]
public class GameHistoryQueryServiceUnitTest
{
    [TestMethod]
    public async Task Normal_Process_SettleDate()
    {
        var query = new GameHistoryQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            true,
            DateTimeOffset.Parse("2022-08-25 16:05:00+08:00"),
            DateTimeOffset.Parse("2022-08-25 16:10:00+08:00"));
        var fakeLog = NullLogger<GameHistoryQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var showBetInfos = new ShowBetInfo[] {
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
            };

        _ = fakeGameApiClient
                .ShowBetListAsync(
                    Arg.Is(query.ApiUrl),
                    Arg.Is(query.PartnerCode),
                    Arg.Is(query.PartnerKey),
                    Arg.Is(query.PartnerIV),
                    Arg.Is(ShowBetListDateType.SettleDate),
                    Arg.Is(query.StartTime),
                    Arg.Is(query.EndTime))
                .Returns(showBetInfos);

        var service = new GameHistoryQueryService(
            fakeLog,
            fakeGameApiClient);

        var actual = await service
            .QueryAsync(
                query)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
    }

    [TestMethod]
    public async Task Normal_Process_BetDate()
    {
        var query = new GameHistoryQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            false,
            DateTimeOffset.Parse("2022-08-25 16:05:00+08:00"),
            DateTimeOffset.Parse("2022-08-25 16:10:00+08:00"));
        var fakeLog = NullLogger<GameHistoryQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var showBetInfos = new ShowBetInfo[] {
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
            };

        _ = fakeGameApiClient
                .ShowBetListAsync(
                    Arg.Is(query.ApiUrl),
                    Arg.Is(query.PartnerCode),
                    Arg.Is(query.PartnerKey),
                    Arg.Is(query.PartnerIV),
                    Arg.Is(ShowBetListDateType.BetDate),
                    Arg.Is(query.StartTime),
                    Arg.Is(query.EndTime))
                .Returns(showBetInfos);

        var service = new GameHistoryQueryService(
            fakeLog,
            fakeGameApiClient);

        var actual = await service
            .QueryAsync(
                query)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
    }

    [TestMethod]
    public async Task Normal_Process_SettleDate_不接受()
    {
        var query = new GameHistoryQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            true,
            DateTimeOffset.Parse("2021-12-04 15:00:00+08:00"),
            DateTimeOffset.Parse("2021-12-04 15:20:00+08:00"));
        var fakeLog = NullLogger<GameHistoryQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var showBetInfos = new ShowBetInfo[] {
                new ShowBetInfo() {
                    MemberId ="29509",
                    Username= "4ueycx9cqj55",
                    RecordDate ="2021-12-04 10:39:53",
                    Reference = "<span style='color: red'>不接受</span>",
                    SportType ="足球",
                    BetType ="滚球大/小",
                    MatchName ="阿拉伯杯2021(在卡塔尔)",
                    HomeName ="黎巴嫩",
                    AwayName ="阿尔及利亚",
                    BetContent = "阿拉伯杯2021(在卡塔尔)<br>黎巴嫩  VS  阿尔及利亚 (0:2)<br>0:1 1.5 小盘 @&nbsp; 0.460",
                    BetAmount ="<s>111.00</s>",
                    BetStatus = "<font color='red'>比分错误</font>",
                    Result = "<font color='red'>比分错误</font>",
                    ActiveAmount ="<s>111.00</s>",
                    ResultScore ="0:0/0:2",
                    SettleDate="2021-12-04 11:06:28",
                    BetIp ="218.250.125.94",
                    InitData = new ShowBetInfoInitData()
                    {
                        FB = "1",
                        Type ="live_highlow",
                        Content ="low@0.46@1.50@0:1",
                        Valid ="-5",
                        LiveStatus ="1",
                        PlanNo ="1",
                        Part ="0",
                        Win ="1",
                        OddsType="1",
                        MatchDate ="2021-12-04 09:00:00",
                        BetAmount ="111.00"
                    }
                }
            };

        _ = fakeGameApiClient
                .ShowBetListAsync(
                    Arg.Is(query.ApiUrl),
                    Arg.Is(query.PartnerCode),
                    Arg.Is(query.PartnerKey),
                    Arg.Is(query.PartnerIV),
                    Arg.Is(ShowBetListDateType.SettleDate),
                    Arg.Is(query.StartTime),
                    Arg.Is(query.EndTime))
                .Returns(showBetInfos);

        var service = new GameHistoryQueryService(
            fakeLog,
            fakeGameApiClient);

        var actual = await service
            .QueryAsync(
                query)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsFalse(actual.Any());
    }

    [TestMethod]
    public async Task Normal_Process_SettleDate_等待确认()
    {
        var query = new GameHistoryQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            true,
            DateTimeOffset.Parse("2021-12-04 15:00:00+08:00"),
            DateTimeOffset.Parse("2021-12-04 15:20:00+08:00"));
        var fakeLog = NullLogger<GameHistoryQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var showBetInfos = new ShowBetInfo[] {
                new ShowBetInfo() {
                    MemberId ="29509",
                    Username= "4ueycx9cqj55",
                    RecordDate ="2021-12-04 10:39:53",
                    Reference = "<span style='color: red'>等待确认</span>",
                    SportType ="足球",
                    BetType ="滚球大/小",
                    MatchName ="阿拉伯杯2021(在卡塔尔)",
                    HomeName ="黎巴嫩",
                    AwayName ="阿尔及利亚",
                    BetContent = "阿拉伯杯2021(在卡塔尔)<br>黎巴嫩  VS  阿尔及利亚 (0:2)<br>0:1 1.5 小盘 @&nbsp; 0.460",
                    BetAmount ="<s>111.00</s>",
                    BetStatus = "<font color='red'>比分错误</font>",
                    Result = "<font color='red'>比分错误</font>",
                    ActiveAmount ="<s>111.00</s>",
                    ResultScore ="0:0/0:2",
                    SettleDate="2021-12-04 11:06:28",
                    BetIp ="218.250.125.94",
                    InitData = new ShowBetInfoInitData()
                    {
                        FB = "1",
                        Type ="live_highlow",
                        Content ="low@0.46@1.50@0:1",
                        Valid ="-5",
                        LiveStatus ="1",
                        PlanNo ="1",
                        Part ="0",
                        Win ="1",
                        OddsType="1",
                        MatchDate ="2021-12-04 09:00:00",
                        BetAmount ="111.00"
                    }
                }
            };

        _ = fakeGameApiClient
                .ShowBetListAsync(
                    Arg.Is(query.ApiUrl),
                    Arg.Is(query.PartnerCode),
                    Arg.Is(query.PartnerKey),
                    Arg.Is(query.PartnerIV),
                    Arg.Is(ShowBetListDateType.SettleDate),
                    Arg.Is(query.StartTime),
                    Arg.Is(query.EndTime))
                .Returns(showBetInfos);

        var service = new GameHistoryQueryService(
            fakeLog,
            fakeGameApiClient);

        var actual = await service
            .QueryAsync(
                query)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsFalse(actual.Any());
    }

    [TestMethod]
    public async Task Normal_Process_SettleDate_Void()
    {
        var query = new GameHistoryQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            true,
            DateTimeOffset.Parse("2023-04-26 21:00:00+08:00"),
            DateTimeOffset.Parse("2023-04-26 21:30:00+08:00"));
        var fakeLog = NullLogger<GameHistoryQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var showBetInfos = new ShowBetInfo[] {
                new ShowBetInfo() {
                    MemberId ="59640",
                    Username= "4uiw4uixe35k",
                    RecordDate ="2023-04-26 21:14:21",
                    Reference = "6042636087",
                    SportType ="足球",
                    BetType ="(滚球)上半场大 / 小",
                    MatchName ="萨尔瓦多甲组联赛",
                    HomeName ="圣特克拉",
                    AwayName ="德拉贡",
                    BetContent = "萨尔瓦多甲组联赛<br>圣特克拉  VS  德拉贡 <br>1:0 1.5 / 2 大盘 @&nbsp; 1.050",
                    BetAmount ="1,523.00",
                    BetStatus = "<br><span style='color: red'>已拒绝</span>",
                    Result = "<span style='color: red'>已拒绝</span>",
                    ActiveAmount ="0.00",
                    ResultScore ="",
                    SettleDate="",
                    BetIp ="124.217.189.154",
                    InitData = new ShowBetInfoInitData()
                    {
                        FB= "1",
                        Type= "live_halftime_highlow",
                        Content= "high@1.05@1.75@1:0",
                        Valid= "1",
                        LiveStatus= "1",
                        PlanNo= "1",
                        Part= "0",
                        Win= "0",
                        OddsType= string.Empty,
                        MatchDate= "2023-04-26 21:00:00",
                        BetAmount="1,523.00"
                    }
                }
            };

        _ = fakeGameApiClient
                .ShowBetListAsync(
                    Arg.Is(query.ApiUrl),
                    Arg.Is(query.PartnerCode),
                    Arg.Is(query.PartnerKey),
                    Arg.Is(query.PartnerIV),
                    Arg.Is(ShowBetListDateType.SettleDate),
                    Arg.Is(query.StartTime),
                    Arg.Is(query.EndTime))
                .Returns(showBetInfos);

        var service = new GameHistoryQueryService(
            fakeLog,
            fakeGameApiClient);

        var actual = await service
            .QueryAsync(
                query)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Single().IsClosed);
        Assert.IsTrue(actual.Single().IsVoid);
        Assert.AreEqual(DateTimeOffset.Parse("2023-04-26 21:14:21-04:00").ToString("O"), actual.Single().ExternalClosedOn);
    }
}
