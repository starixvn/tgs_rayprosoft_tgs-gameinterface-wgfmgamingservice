using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;
using Tgs.GameInterface.WGFmGamingService.Core.Models;

namespace Tgs.GameInterface.WGFmGamingService.Core.UnitTest.ApplicationServicesUnitTest;

public class GameHistoryQueryServiceUnitTest
{
    [Fact]
    public async Task Win_Is_1_BetStatus_Is_Not_Accepted()
    {
        var query = new GameHistoryQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            true,
            DateTimeOffset.Parse("2022-08-25T16:05:00+08:00"),
            DateTimeOffset.Parse("2022-08-25T16:10:00+08:00"));
        var fakeLog = NullLogger<GameHistoryQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var showBetInfos = new ShowBetInfo[] {
            new() {
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
            Arg.Is(query.EndTime),
            Arg.Any<CancellationToken>())
            .Returns(showBetInfos);

        var service = new GameHistoryQueryService(
            fakeLog,
            fakeGameApiClient);

        var actual = await service.QueryAsync(
            query: query)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameHistoryResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal("4ufjeyz80v90", actual.First().PlayerCode);
        Assert.Equal("8082591913", actual.First().ExternalRoundId);
        Assert.True(actual.First().IsClosed);
        Assert.True(actual.First().IsVoid);
        Assert.Equal(DateTimeOffset.Parse("2022-08-25T02:44:52-04:00").ToString("O"), actual.First().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2022-08-25T04:05:13-04:00").ToString("O"), actual.First().ExternalClosedOn);
        Assert.Equal(50m, actual.First().Bet);
        Assert.Equal(50m, actual.First().ValidBet);
        Assert.Equal(0m, actual.First().Turnover);
        Assert.Equal(0m, actual.First().PlayerWinLoss);
        Assert.Equal(0m, actual.First().Settle);
        Assert.Equal(JsonSerializer.Serialize(showBetInfos.First()), actual.First().AdditionalData);
    }

    [Fact]
    public async Task Win_Is_1_BetStatus_Is_Settled_Winloss_is_Loss()
    {
        var query = new GameHistoryQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            true,
            DateTimeOffset.Parse("2022-08-25T16:05:00+08:00"),
            DateTimeOffset.Parse("2022-08-25T16:10:00+08:00"));
        var fakeLog = NullLogger<GameHistoryQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var showBetInfos = new ShowBetInfo[] {
                new() {
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

        var actual = await service.QueryAsync(
            query: query)
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Equal("4ufjeyz80v90", actual.First().PlayerCode);
        Assert.Equal("3082562817", actual.First().ExternalRoundId);
        Assert.True(actual.First().IsClosed);
        Assert.False(actual.First().IsVoid);
        Assert.Equal(DateTimeOffset.Parse("2022-08-25T02:46:20-04:00").ToString("O"), actual.First().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2022-08-25T04:05:13-04:00").ToString("O"), actual.First().ExternalClosedOn);
        Assert.Equal(50m, actual.First().Bet);
        Assert.Equal(50m, actual.First().ValidBet);
        Assert.Equal(50m, actual.First().Turnover);
        Assert.Equal(-49.63m, actual.First().PlayerWinLoss);
        Assert.Equal(0.37m, actual.First().Settle);
        Assert.Equal(JsonSerializer.Serialize(showBetInfos.First()), actual.First().AdditionalData);
    }

    [Fact]
    public async Task Win_Is_1_BetStatus_Is_Settled_Winloss_is_Win()
    {
        var query = new GameHistoryQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            true,
            DateTimeOffset.Parse("2022-08-25T16:05:00+08:00"),
            DateTimeOffset.Parse("2022-08-25T16:10:00+08:00"));
        var fakeLog = NullLogger<GameHistoryQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var showBetInfos = new ShowBetInfo[] {
            new() {
                MemberId = "55017",
                Username = "4ufjeyz80v90",
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
            apiUrl: Arg.Is(query.ApiUrl),
            partnerCode: Arg.Is(query.PartnerCode),
            partnerKey: Arg.Is(query.PartnerKey),
            partnerIV: Arg.Is(query.PartnerIV),
            dateType: Arg.Is(ShowBetListDateType.SettleDate),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime))
            .Returns(showBetInfos);

        var service = new GameHistoryQueryService(
            fakeLog,
            fakeGameApiClient);

        var actual = await service.QueryAsync(
            query: query)
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Equal("4ufjeyz80v90", actual.First().PlayerCode);
        Assert.Equal("6082524611", actual.First().ExternalRoundId);
        Assert.True(actual.First().IsClosed);
        Assert.False(actual.First().IsVoid);
        Assert.Equal(DateTimeOffset.Parse("2022-08-25T00:38:47-04:00").ToString("O"), actual.First().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2022-08-25T04:05:13-04:00").ToString("O"), actual.First().ExternalClosedOn);
        Assert.Equal(260.00m, actual.First().Bet);
        Assert.Equal(260.00m, actual.First().ValidBet);
        Assert.Equal(260.00m, actual.First().Turnover);
        Assert.Equal(170.27m, actual.First().PlayerWinLoss);
        Assert.Equal(430.27m, actual.First().Settle);
        Assert.Equal(JsonSerializer.Serialize(showBetInfos.First()), actual.First().AdditionalData);
    }

    [Fact]
    public async Task Win_Is_1_BetStatus_Is_Rejected()
    {
        var query = new GameHistoryQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            true,
            DateTimeOffset.Parse("2023-07-10T06:30:00+08:00"),
            DateTimeOffset.Parse("2023-07-10T06:40:00+08:00"));
        var fakeLog = NullLogger<GameHistoryQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var showBetInfos = new ShowBetInfo[] {
                new() {
                    MemberId ="65809",
                    Username= "4ukcrrmd6683",
                    RecordDate ="2023-07-10 22:34:33",
                    Reference = "5071046073",
                    SportType ="足球",
                    BetType ="(滚球)上半场大 / 小",
                    MatchName ="墨西哥女子冠军赛",
                    HomeName ="塔格雷斯(女)",
                    AwayName ="阿美利加会(女)",
                    BetContent = "墨西哥女子冠军赛<br>塔格雷斯(女)  VS  阿美利加会(女) (上半场1:0)<br>0:0 0.5 小盘 @&nbsp; 0.600",
                    BetAmount ="3,333.00",
                    BetStatus = "<br><span style='color: red'>已拒绝</span>",
                    Result = "<span style='color: red'>已拒绝</span>",
                    ActiveAmount ="0.00",
                    ResultScore ="1:0/-:-",
                    SettleDate="2023-07-10 23:05:04",
                    BetIp =  "203.145.94.7",
                    InitData = new ShowBetInfoInitData()
                    {
                        FB = "1",
                        Type ="live_halftime_highlow",
                        Content ="low@0.60@0.50@0:0",
                        Valid ="1",
                        LiveStatus = "1",
                        PlanNo ="1",
                        Part = "0",
                        Win ="1",
                        OddsType=string.Empty,
                        MatchDate ="2023-07-10 22:00:00",
                        BetAmount ="3,333.00"
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

        var actual = await service.QueryAsync(
            query)
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Equal("4ukcrrmd6683", actual.First().PlayerCode);
        Assert.Equal("5071046073", actual.First().ExternalRoundId);
        Assert.True(actual.First().IsClosed);
        Assert.True(actual.First().IsVoid);
        Assert.Equal(DateTimeOffset.Parse("2023-07-10T22:34:33-04:00").ToString("O"), actual.First().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2023-07-10T23:05:04-04:00").ToString("O"), actual.First().ExternalClosedOn);
        Assert.Equal(3333.00m, actual.First().Bet);
        Assert.Equal(3333.00m, actual.First().ValidBet);
        Assert.Equal(0m, actual.First().Turnover);
        Assert.Equal(0m, actual.First().PlayerWinLoss);
        Assert.Equal(0m, actual.First().Settle);
        Assert.Equal(JsonSerializer.Serialize(showBetInfos.First()), actual.First().AdditionalData);
    }

    [Fact]
    public async Task Win_Is_0_BetStatus_Is_Unsettled()
    {
        var query = new GameHistoryQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            false,
            DateTimeOffset.Parse("2022-08-25T16:05:00+08:00"),
            DateTimeOffset.Parse("2022-08-25T16:10:00+08:00"));
        var fakeLog = NullLogger<GameHistoryQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var showBetInfos = new ShowBetInfo[] {
                new() {
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

        var actual = await service.QueryAsync(
            query)
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Equal("4ued4qr92gm0", actual.First().PlayerCode);
        Assert.Equal("4083093533", actual.First().ExternalRoundId);
        Assert.False(actual.First().IsClosed);
        Assert.False(actual.First().IsVoid);
        Assert.Equal(DateTimeOffset.Parse("2022-08-30T02:21:40-04:00").ToString("O"), actual.First().ExternalCreatedOn);
        Assert.Equal(string.Empty, actual.First().ExternalClosedOn);
        Assert.Equal(50.00m, actual.First().Bet);
        Assert.Equal(50.00m, actual.First().ValidBet);
        Assert.Equal(0.0m, actual.First().Turnover);
        Assert.Equal(0.0m, actual.First().PlayerWinLoss);
        Assert.Equal(0.0m, actual.First().Settle);
        Assert.Equal(JsonSerializer.Serialize(showBetInfos.First()), actual.First().AdditionalData);
    }

    [Fact]
    public async Task Win_Is_1_Valid_Is_5_BetStatus_Is_Score_Error()
    {
        var query = new GameHistoryQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            true,
            DateTimeOffset.Parse("2021-12-04T15:00:00+08:00"),
            DateTimeOffset.Parse("2021-12-04T15:20:00+08:00"));
        var fakeLog = NullLogger<GameHistoryQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var showBetInfos = new ShowBetInfo[] {
            new() {
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
            apiUrl: Arg.Is(query.ApiUrl),
            partnerCode: Arg.Is(query.PartnerCode),
            partnerKey: Arg.Is(query.PartnerKey),
            partnerIV: Arg.Is(query.PartnerIV),
            dateType: Arg.Is(ShowBetListDateType.SettleDate),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime))
            .Returns(showBetInfos);

        var service = new GameHistoryQueryService(
            fakeLog,
            fakeGameApiClient);

        var actual = await service.QueryAsync(
            query: query)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameHistoryResult>();

        Assert.NotNull(actual);
        Assert.False(actual.Any());
    }

    [Fact]
    public async Task Win_Is_1_Valid_Is_5_Reference_Is_Waiting_For_Confirmation()
    {
        var query = new GameHistoryQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            true,
            DateTimeOffset.Parse("2021-12-04T15:00:00+08:00"),
            DateTimeOffset.Parse("2021-12-04T15:20:00+08:00"));
        var fakeLog = NullLogger<GameHistoryQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var showBetInfos = new ShowBetInfo[] {
                new() {
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

        var actual = await service.QueryAsync(
            query)
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.Empty(actual);
    }

    [Fact]
    public async Task Win_Is_0_BetStatus_Is_Rejected()
    {
        var query = new GameHistoryQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            true,
            DateTimeOffset.Parse("2023-04-26T21:00:00+08:00"),
            DateTimeOffset.Parse("2023-04-26T21:30:00+08:00"));
        var fakeLog = NullLogger<GameHistoryQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var showBetInfos = new ShowBetInfo[] {
                new() {
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

        var actual = await service.QueryAsync(
            query)
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
        Assert.Equal("4uiw4uixe35k", actual.First().PlayerCode);
        Assert.Equal("6042636087", actual.First().ExternalRoundId);
        Assert.True(actual.First().IsClosed);
        Assert.True(actual.First().IsVoid);
        Assert.Equal(DateTimeOffset.Parse("2023-04-26T21:14:21-04:00").ToString("O"), actual.First().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2023-04-26T21:14:21-04:00").ToString("O"), actual.First().ExternalClosedOn);
        Assert.Equal(1523m, actual.First().Bet);
        Assert.Equal(1523m, actual.First().ValidBet);
        Assert.Equal(0m, actual.First().Turnover);
        Assert.Equal(0m, actual.First().PlayerWinLoss);
        Assert.Equal(0m, actual.First().Settle);
        Assert.Equal(JsonSerializer.Serialize(showBetInfos.First()), actual.First().AdditionalData);
    }

    [Fact]
    public async Task Win_Is_0_BetStatus_is_Waiting_For_Confirmation()
    {
        var query = new GameHistoryQuery(
            new Uri("http://localhost:8088/"),
            "partnerCode",
            "partnerKey",
            "partnerIV",
            true,
            DateTimeOffset.Parse("2023-07-06T13:28:00+08:00"),
            DateTimeOffset.Parse("2023-07-06T13:30:00+08:00"));
        var fakeLog = NullLogger<GameHistoryQueryService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var showBetInfos = new ShowBetInfo[] {
                new() {
                    MemberId ="65809",
                    Username= "4ukcrrmd6683",
                    RecordDate ="2023-07-06 13:28:59",
                    Reference = "8070673197",
                    SportType ="足球",
                    BetType ="(滚球)让球",
                    MatchName ="球会友谊赛",
                    HomeName ="博霍尔特",
                    AwayName ="沙尔克04",
                    BetContent = "球会友谊赛<br>沙尔克04 0.5 博霍尔特 <br> 2:2 博霍尔特 @&nbsp; 0.930",
                    BetAmount ="1,000.00",
                    BetStatus = "<br><span style='color: red'>等待确认</span>",
                    Result = "<span style='color: red'>等待确认</span>",
                    ActiveAmount ="0.00",
                    ResultScore ="",
                    SettleDate="",
                    BetIp = "203.145.94.7",
                    InitData = new ShowBetInfoInitData()
                    {
                        FB = "1",
                        Type ="live",
                        Content ="home@0.93@0.5@2:2",
                        Valid ="1",
                        LiveStatus ="0",
                        PlanNo ="1",
                        Part ="0",
                        Win ="0",
                        OddsType="1",
                        MatchDate ="2023-07-06 12:06:00",
                        BetAmount ="1,000.00"
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

        var actual = await service.QueryAsync(
            query)
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.Empty(actual);
    }
}
