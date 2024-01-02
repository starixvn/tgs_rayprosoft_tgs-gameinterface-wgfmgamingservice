using Grpc.Core;
using Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;
using Tgs.GameInterface.WGFmGamingService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.GrpcServices;

public class WGFmGamingGrpcService(
    ILogger<WGFmGamingGrpcService> logger,
    ICommandService<FundInCommand> fundInService,
    ICommandService<FundOutCommand> fundOutService,
    IGetService<FundStatusQuery, bool> fundStatusService,
    IQueryService<GameHistoryQuery, GameHistoryResult> gameHistoryService,
    IGetService<GameLoginQuery, Uri> gameLoginService,
    IGetService<GamePrepareLoginQuery, string> gamePrepareLoginService,
    IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
    ICommandService<PlayerKickCommand> playerKickLineService) : GameInterfaceWGFmGamingService.GameInterfaceWGFmGamingServiceBase
{
    public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
    {
        logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

        return Task.FromResult(new CheckingServiceReply
        {
            MachineName = Environment.MachineName
        });
    }

    public override async Task<FundReply> FundIn(
        FundRequest request,
        ServerCallContext context)
    {
        var command = new FundInCommand(
            new(request.ApiUrl),
            request.PartnerCode,
            request.PartnerKey,
            request.PartnerIv,
            request.PlayerCode,
            request.OrderId,
            request.Amount.ToDecimal()!.Value);

        await fundInService
             .ExecuteAsync(
                 command,
                 context.CancellationToken)
             .ConfigureAwait(false);

        return new FundReply
        {
            Success = true
        };
    }

    public override async Task<FundReply> FundOut(
        FundRequest request,
        ServerCallContext context)
    {
        var command = new FundOutCommand(
            new(request.ApiUrl),
            request.PartnerCode,
            request.PartnerKey,
            request.PartnerIv,
            request.PlayerCode,
            request.OrderId,
            request.Amount.ToDecimal()!.Value);

        await fundOutService
             .ExecuteAsync(
                 command,
                 context.CancellationToken)
             .ConfigureAwait(false);

        return new FundReply
        {
            Success = true
        };
    }

    public override async Task<FundStatusReply> FundStatus(
        FundStatusRequest request,
        ServerCallContext context)
    {
        var query = new FundStatusQuery(
            new(request.ApiUrl),
            request.PartnerCode,
            request.PartnerKey,
            request.PartnerIv,
            request.OrderId);

        var result = await fundStatusService
            .GetAsync(
                query,
                context.CancellationToken)
            .ConfigureAwait(false);

        return new FundStatusReply()
        {
            Success = result
        };
    }

    public override async Task<GameHistoryReply> GameHistory(
        GameHistoryRequest request,
        ServerCallContext context)
    {
        var query = new GameHistoryQuery(
            new(request.ApiUrl),
            request.PartnerCode,
            request.PartnerKey,
            request.PartnerIv,
            request.IsSettle,
            request.StartTime.ToDateTimeOffset(),
            request.EndTime.ToDateTimeOffset());

        var results = await gameHistoryService
            .QueryAsync(
                query,
                context.CancellationToken)
            .ToArrayAsync()
            .ConfigureAwait(false);

        var reply = new GameHistoryReply();

        if (results.Length != 0)
        {
            foreach (var result in results)
            {
                reply.Items.Add(new GameHistoryInfo()
                {
                    PlayerCode = result.PlayerCode,
                    ExternalRoundId = result.ExternalRoundId,
                    ExternalGameId = result.ExternalGameId,
                    ExternalCreatedOn = result.ExternalCreatedOn,
                    ExternalClosedOn = result.ExternalClosedOn,
                    IsClosed = result.IsClosed,
                    IsVoid = result.IsVoid,
                    Bet = result.Bet,
                    ValidBet = result.ValidBet,
                    PlayerWinLoss = result.PlayerWinLoss,
                    Turnover = result.Turnover,
                    Settle = result.Settle,
                    AdditionalData = result.AdditionalData
                });
            }
        }

        return reply;
    }

    public override async Task<GameLoginReply> GameLogin(
        GameLoginRequest request,
        ServerCallContext context)
    {
        var query = new GameLoginQuery(
            new(request.ApiUrl),
            request.PartnerCode,
            request.Token,
            request.Lang,
            request.Version,
            request.IsPhone);

        var result = await gameLoginService
            .GetAsync(
                query,
                context.CancellationToken)
            .ConfigureAwait(false);

        return new GameLoginReply()
        {
            GameUrl = result.ToString()
        };
    }

    public override async Task<GamePrepareLoginReply> GamePrepareLogin(
        GamePrepareLoginRequest request,
        ServerCallContext context)
    {
        var query = new GamePrepareLoginQuery(
            new(request.ApiUrl),
            request.PartnerCode,
            request.PartnerKey,
            request.PartnerIv,
            request.PlayerCode,
            request.CurrencyCode);

        var result = await gamePrepareLoginService
            .GetAsync(
                query,
                context.CancellationToken)
            .ConfigureAwait(false);

        return new GamePrepareLoginReply()
        {
            Token = result
        };
    }

    public override async Task<PlayerBalanceReply> PlayerBalance(
        PlayerBalanceRequest request,
        ServerCallContext context)
    {
        var query = new PlayerBalanceQuery(
            new(request.ApiUrl),
            request.PartnerCode,
            request.PartnerKey,
            request.PartnerIv,
            request.PlayerCode);

        var result = await playerBalanceService
                        .GetAsync(
                            query,
                            context.CancellationToken)
                        .ConfigureAwait(false);

        return new PlayerBalanceReply()
        {
            IsPlayer = result.IsPlayer,
            Balance = result.Balance
        };
    }

    public override async Task<PlayerKickReply> PlayerKick(
        PlayerKickRequest request,
        ServerCallContext context)
    {
        var command = new PlayerKickCommand(
            new(request.ApiUrl),
            request.PartnerCode,
            request.PartnerKey,
            request.PartnerIv,
            request.PlayerCode);

        await playerKickLineService
            .ExecuteAsync(
                command,
                context.CancellationToken)
            .ConfigureAwait(false);

        return new PlayerKickReply()
        {
            Success = true
        };
    }
}
