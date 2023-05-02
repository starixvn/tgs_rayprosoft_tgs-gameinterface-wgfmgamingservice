using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices
{
    internal class GameHistoryQueryService : IQueryService<GameHistoryQuery, GameHistoryResult>
    {
        private readonly ILogger<GameHistoryQueryService> _logger;
        private readonly IGameApiClient _client;
        private readonly TimeZoneInfo _timeZone;

        public GameHistoryQueryService(
            ILogger<GameHistoryQueryService> logger,
            IGameApiClient client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _timeZone = TimeZoneInfo.CreateCustomTimeZone(id: "eastern time", baseUtcOffset: TimeSpan.FromHours(-4), "eastern time", "eastern time");
        }

        public async IAsyncEnumerable<GameHistoryResult> QueryAsync(
            GameHistoryQuery query,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var showBetInfos = await _client
                        .ShowBetListAsync(
                            query.ApiUrl,
                            query.PartnerCode,
                            query.PartnerKey,
                            query.PartnerIV,
                            query.IsSettle ? ShowBetListDateType.SettleDate : ShowBetListDateType.BetDate,
                            query.StartTime,
                            query.EndTime,
                            cancellationToken)
                        .ConfigureAwait(false);

            if (!showBetInfos.Any())
            {
                yield break;
            }

            foreach (var showBetInfo in showBetInfos)
            {
                if (showBetInfo.Reference.Contains("不接受", StringComparison.OrdinalIgnoreCase)
                    || showBetInfo.Reference.Contains("等待确认", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogError("There is an abnormal bet.({showBetInfo})", JsonSerializer.Serialize(showBetInfo));
                    continue;
                }
                var isClosed = !string.Equals(showBetInfo.InitData.Win, "0", StringComparison.OrdinalIgnoreCase);
                var isVoid = !decimal.TryParse(showBetInfo.Result, out var playerWinLoss);
                _ = decimal.TryParse(showBetInfo.InitData.BetAmount, out var bet);
                var turnover = !isClosed || isVoid ? 0 : bet;
                playerWinLoss = !isClosed || isVoid ? 0 : playerWinLoss;
                var settle = !isClosed || isVoid ? 0 : bet + playerWinLoss;
                var externalCreatedOn = new DateTimeOffset(DateTime.Parse(showBetInfo.RecordDate), _timeZone.GetUtcOffset(DateTime.Parse(showBetInfo.RecordDate))).ToString("O");
                var externalClosedOn = (isClosed) ? new DateTimeOffset(DateTime.Parse(showBetInfo.SettleDate), _timeZone.GetUtcOffset(DateTime.Parse(showBetInfo.SettleDate))).ToString("O") : string.Empty;

                if (!isClosed && isVoid)
                {
                    externalClosedOn = new DateTimeOffset(DateTime.Parse(showBetInfo.RecordDate), _timeZone.GetUtcOffset(DateTime.Parse(showBetInfo.RecordDate))).ToString("O");
                    isClosed = true;
                }
                yield return new GameHistoryResult(
                    PlayerCode: showBetInfo.Username,
                    ExternalRoundId: showBetInfo.Reference,
                    ExternalGameId: "1",
                    ExternalCreatedOn: externalCreatedOn,
                    ExternalClosedOn: externalClosedOn,
                    IsClosed: isClosed,
                    IsVoid: isVoid,
                    Bet: bet,
                    ValidBet: bet,
                    PlayerWinLoss: playerWinLoss,
                    Turnover: turnover,
                    Settle: settle,
                    AdditionalData: JsonSerializer.Serialize(showBetInfo));
            }
        }
    }
}
