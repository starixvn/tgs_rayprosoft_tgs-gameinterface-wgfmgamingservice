using Grpc.Core;
using Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;
using Tgs.GameInterface.WGFmGamingService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.GrpcServices
{
    public class WGFmGamingGrpcService : GameInterfaceWGFmGamingService.GameInterfaceWGFmGamingServiceBase
    {
        private readonly ICommandService<FundInCommand> _fundInService;
        private readonly ICommandService<FundOutCommand> _fundOutService;
        private readonly IGetService<FundStatusQuery, bool> _fundStatusService;
        private readonly IQueryService<GameHistoryQuery, GameHistoryResult> _gameHistoryService;
        private readonly IGetService<GameLoginQuery, Uri> _gameLoginService;
        private readonly IGetService<GamePrepareLoginQuery, string> _gamePrepareLoginService;
        private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
        private readonly ICommandService<PlayerKickLineCommand> _playerKickLineService;

        public WGFmGamingGrpcService(
            ICommandService<FundInCommand> fundInService,
            ICommandService<FundOutCommand> fundOutService,
            IGetService<FundStatusQuery, bool> fundStatusService,
            IQueryService<GameHistoryQuery, GameHistoryResult> gameHistoryService,
            IGetService<GameLoginQuery, Uri> gameLoginService,
            IGetService<GamePrepareLoginQuery, string> gamePrepareLoginService,
            IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
            ICommandService<PlayerKickLineCommand> playerKickLineService
            )
        {
            _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
            _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
            _fundStatusService = fundStatusService ?? throw new ArgumentNullException(nameof(fundStatusService));
            _gameHistoryService = gameHistoryService ?? throw new ArgumentNullException(nameof(gameHistoryService));
            _gameLoginService = gameLoginService ?? throw new ArgumentNullException(nameof(gameLoginService));
            _gamePrepareLoginService = gamePrepareLoginService ?? throw new ArgumentNullException(nameof(gamePrepareLoginService));
            _playerBalanceService = playerBalanceService ?? throw new ArgumentNullException(nameof(playerBalanceService));
            _playerKickLineService = playerKickLineService ?? throw new ArgumentNullException(nameof(playerKickLineService));
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

            await _fundInService
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

            await _fundOutService
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

            var result = await _fundStatusService
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

            var results = await _gameHistoryService
                .QueryAsync(
                    query,
                    context.CancellationToken)
                .ToArrayAsync()
                .ConfigureAwait(false);

            var reply = new GameHistoryReply();

            if (results.Any())
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
                request.Lang);

            var result = await _gameLoginService
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

            var result = await _gamePrepareLoginService
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

            var result = await _playerBalanceService
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

        public override async Task<PlayerKickLineReply> PlayerKickLine(
            PlayerKickLineRequest request,
            ServerCallContext context)
        {
            var command = new PlayerKickLineCommand(
                new(request.ApiUrl),
                request.PartnerCode,
                request.PartnerKey,
                request.PartnerIv,
                request.PlayerCode);

            await _playerKickLineService
                .ExecuteAsync(
                    command,
                    context.CancellationToken)
                .ConfigureAwait(false);

            return new PlayerKickLineReply()
            {
                Success = true
            };
        }
    }
}
