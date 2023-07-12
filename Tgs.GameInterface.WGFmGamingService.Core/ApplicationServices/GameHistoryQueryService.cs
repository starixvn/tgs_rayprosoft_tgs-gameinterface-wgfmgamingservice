using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.WGFmGamingService.Core.Extensions;
using Tgs.GameInterface.WGFmGamingService.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;

internal class GameHistoryQueryService : IQueryService<GameHistoryQuery, GameHistoryResult>
{
    private readonly ILogger<GameHistoryQueryService> _logger;
    private readonly IGameApiClient _client;

    public GameHistoryQueryService(
        ILogger<GameHistoryQueryService> logger,
        IGameApiClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameHistoryResult> QueryAsync(
        GameHistoryQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var showBetInfos = await _client
            .ShowBetListAsync(
            apiUrl: query.ApiUrl,
            partnerCode: query.PartnerCode,
            partnerKey: query.PartnerKey,
            partnerIV: query.PartnerIV,
            dateType: query.IsSettle ? ShowBetListDateType.SettleDate : ShowBetListDateType.BetDate,
            startTime: query.StartTime,
            endTime: query.EndTime,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<ShowBetInfo>();

        if (!showBetInfos.Any())
        {
            yield break;
        }

        foreach (var showBetInfo in showBetInfos)
        {
            if (
                showBetInfo.Reference.Contains("不接受", StringComparison.OrdinalIgnoreCase) ||
                showBetInfo.Reference.Contains("等待确认", StringComparison.OrdinalIgnoreCase) ||
                showBetInfo.BetStatus.Contains("比分错误", StringComparison.OrdinalIgnoreCase) ||
                showBetInfo.BetStatus.Contains("等待确认", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogError("There is an abnormal bet.({showBetInfo})", JsonSerializer.Serialize(showBetInfo));
                continue;
            }
            var isClosed = string.Equals(showBetInfo.InitData.Win, "1", StringComparison.OrdinalIgnoreCase);
            var isVoid = false;
            if (showBetInfo.BetStatus.Contains("不接受", StringComparison.OrdinalIgnoreCase) ||
                showBetInfo.BetStatus.Contains("已拒绝", StringComparison.OrdinalIgnoreCase))
            {
                isClosed = true;
                isVoid = true;
            }
            var playerCode = showBetInfo.Username;
            var externalRoundId = showBetInfo.Reference;
            var externalCreatedOn = showBetInfo.RecordDate.StringConvertDateTimeOffset().ToString("O");
            var externalClosedOn = isClosed
                ? string.IsNullOrEmpty(showBetInfo.SettleDate)
                    ? externalCreatedOn
                    : showBetInfo.SettleDate.StringConvertDateTimeOffset().ToString("O")
                : string.Empty;

            var bet = Convert.ToDecimal(showBetInfo.InitData.BetAmount);
            var turnover = 0.0m;
            var settle = 0.0m;
            var playerWinLoss = 0.0m;

            if (isClosed && !isVoid)
            {
                turnover = bet;
                playerWinLoss = Convert.ToDecimal(showBetInfo.Result);
                settle = bet + playerWinLoss;
            }

            yield return new GameHistoryResult(
                PlayerCode: playerCode,
                ExternalRoundId: externalRoundId,
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
