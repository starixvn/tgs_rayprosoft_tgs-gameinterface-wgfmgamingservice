using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.WGFmGamingService.Core.ApplicationServices;
using Tgs.GameInterface.WGFmGamingService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGFmGamingService.Controllers;

[Route("wgfm")]
[ApiController]
public class WGFmGamingController : ControllerBase
{
    /// <summary>
    /// Funds the in asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.jsc080.net/",
    ///             "partnerCode": "TA",
    ///             "partnerKey": "abc1238888888888",
    ///             "partnerIV": "cba1238888888888",
    ///             "playerCode": "4ucmqplxb029",
    ///             "orderId": "2209010937",
    ///             "amount": "1"
    ///         }
    /// </remarks>
    [HttpPost("fund/in")]
    public ValueTask FundInAsync(
        [FromServices] ICommandService<FundInCommand> service,
        FundViewModel source)
    {
        var command = new FundInCommand(
            source.ApiUrl,
            source.PartnerCode,
            source.PartnerKey,
            source.PartnerIV,
            source.PlayerCode,
            source.OrderId,
            source.Amount);

        return service
            .ExecuteAsync(
                command,
                HttpContext.RequestAborted);
    }

    /// <summary>
    /// Funds the out asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.jsc080.net/",
    ///             "partnerCode": "TA",
    ///             "partnerKey": "abc1238888888888",
    ///             "partnerIV": "cba1238888888888",
    ///             "playerCode": "4ucmqplxb029",
    ///             "orderId": "2209010938",
    ///             "amount": "1"
    ///         }
    /// </remarks>
    [HttpPost("fund/out")]
    public ValueTask FundOutAsync(
        [FromServices] ICommandService<FundOutCommand> service,
        FundViewModel source)
    {
        var command = new FundOutCommand(
            source.ApiUrl,
            source.PartnerCode,
            source.PartnerKey,
            source.PartnerIV,
            source.PlayerCode,
            source.OrderId,
            source.Amount);

        return service
            .ExecuteAsync(
                command,
                HttpContext.RequestAborted);
    }

    /// <summary>
    /// Funds the status asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.jsc080.net/",
    ///             "partnerCode": "TA",
    ///             "partnerKey": "abc1238888888888",
    ///             "partnerIV": "cba1238888888888",
    ///             "orderId": "2209010938"
    ///         }
    /// </remarks>
    [HttpPost("fund/status")]
    public ValueTask<bool> FundStatusAsync(
        [FromServices] IGetService<FundStatusQuery, bool> service,
        FundStatusViewModel source)
    {
        var query = new FundStatusQuery(
            source.ApiUrl,
            source.PartnerCode,
            source.PartnerKey,
            source.PartnerIV,
            source.OrderId);

        return service
            .GetAsync(
                query,
                HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the balance asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.jsc080.net/",
    ///             "partnerCode": "TA",
    ///             "partnerKey": "abc1238888888888",
    ///             "partnerIV": "cba1238888888888",
    ///             "playerCode": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("player/balance")]
    public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
        [FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
        PlayerBalanceViewModel source)
    {
        var query = new PlayerBalanceQuery(
            source.ApiUrl,
            source.PartnerCode,
            source.PartnerKey,
            source.PartnerIV,
            source.PlayerCode);

        var result = await service
                .GetAsync(
                    query,
                    HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new PlayerBalanceResultViewModel(
            result.IsPlayer,
            result.Balance);
    }

    /// <summary>
    /// Games the login asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.jsc080.net/",
    ///             "partnerCode": "TA",
    ///             "token": "{token}",
    ///             "lang": "0",
    ///             "version": 2
    ///         }
    /// </remarks>
    [HttpPost("game/login")]
    public ValueTask<Uri> GameLoginAsync(
        [FromServices] IGetService<GameLoginQuery, Uri> service,
        GameLoginViewModel source)
    {
        var query = new GameLoginQuery(
            source.ApiUrl,
            source.PartnerCode,
            source.Token,
            source.Lang,
            source.Version);

        return service
            .GetAsync(
                query,
                HttpContext.RequestAborted);
    }

    /// <summary>
    /// Games the prepare login asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.jsc080.net/",
    ///             "partnerCode": "TA",
    ///             "partnerKey": "abc1238888888888",
    ///             "partnerIV": "cba1238888888888",
    ///             "playerCode": "4ucmqplxb029",
    ///             "currencyCode": "RMB"
    ///         }
    /// </remarks>
    [HttpPost("game/preparelogin")]
    public ValueTask<string> GamePrepareLoginAsync(
        [FromServices] IGetService<GamePrepareLoginQuery, string> service,
        GamePrepareLoginViewModel source)
    {
        var query = new GamePrepareLoginQuery(
            source.ApiUrl,
            source.PartnerCode,
            source.PartnerKey,
            source.PartnerIV,
            source.PlayerCode,
            source.CurrencyCode);

        return service
            .GetAsync(
                query,
                HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the kick asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.jsc080.net/",
    ///             "partnerCode": "TA",
    ///             "partnerKey": "abc1238888888888",
    ///             "partnerIV": "cba1238888888888",
    ///             "playerCode": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("player/kick")]
    public ValueTask PlayerKickAsync(
        [FromServices] ICommandService<PlayerKickCommand> service,
        PlayerKickModel source)
    {
        var command = new PlayerKickCommand(
            source.ApiUrl,
            source.PartnerCode,
            source.PartnerKey,
            source.PartnerIV,
            source.PlayerCode);

        return service
            .ExecuteAsync(
                command,
                HttpContext.RequestAborted);
    }

    /// <summary>
    /// Games the history asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.jsc080.net/",
    ///             "partnerCode": "TA",
    ///             "partnerKey": "abc1238888888888",
    ///             "partnerIV": "cba1238888888888",
    ///             "isSettle": true,
    ///             "startTime": "2022-08-25T16:05:00+08:00",
    ///             "endTime": "2022-08-25T16:10:00+08:00"
    ///         }
    /// </remarks>
    [HttpPost("game/history")]
    public async IAsyncEnumerable<GameHistoryResultViewModel> GameHistoryAsync(
        [FromServices] IQueryService<GameHistoryQuery, GameHistoryResult> service,
        GameHistoryViewModel source)
    {
        var query = new GameHistoryQuery(
            source.ApiUrl,
            source.PartnerCode,
            source.PartnerKey,
            source.PartnerIV,
            source.IsSettle,
            source.StartTime,
            source.EndTime);

        var results = await service
            .QueryAsync(
                query,
                HttpContext.RequestAborted)
            .ToArrayAsync()
            .ConfigureAwait(false);

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            yield return new GameHistoryResultViewModel(
                PlayerCode: result.PlayerCode,
                ExternalRoundId: result.ExternalRoundId,
                ExternalGameId: result.ExternalGameId,
                ExternalCreatedOn: result.ExternalCreatedOn,
                ExternalClosedOn: result.ExternalClosedOn,
                IsClosed: result.IsClosed,
                IsVoid: result.IsVoid,
                Bet: result.Bet,
                ValidBet: result.ValidBet,
                PlayerWinLoss: result.PlayerWinLoss,
                Turnover: result.Turnover,
                Settle: result.Settle,
                AdditionalData: result.AdditionalData);
        }
    }
}
