using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.WGFmGamingService.Protos;

namespace Tgs.GameInterface.WGFmGamingService.Client;

internal class GameInterfaceWGFmGamingClient : IGameInterfaceWGFmGamingClient
{
    private readonly GameInterfaceWGFmGamingService.GameInterfaceWGFmGamingServiceClient _client;

    public GameInterfaceWGFmGamingClient(GameInterfaceWGFmGamingService.GameInterfaceWGFmGamingServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<string> CheckingServiceAsync(
        string machineName,
        CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await _client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask FundInAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string playerCode,
        string orderId,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var request = new FundRequest()
        {
            ApiUrl = apiUrl.ToString(),
            PartnerCode = partnerCode,
            PartnerKey = partnerKey,
            PartnerIv = partnerIV,
            PlayerCode = playerCode,
            OrderId = orderId,
            Amount = amount
        };

        _ = await _client
             .FundInAsync(
                 request: request,
                 cancellationToken: cancellationToken)
             .ConfigureAwait(false);
    }

    public async ValueTask FundOutAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string playerCode,
        string orderId,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var request = new FundRequest()
        {
            ApiUrl = apiUrl.ToString(),
            PartnerCode = partnerCode,
            PartnerKey = partnerKey,
            PartnerIv = partnerIV,
            PlayerCode = playerCode,
            OrderId = orderId,
            Amount = amount
        };

        _ = await _client
             .FundOutAsync(
                 request: request,
                 cancellationToken: cancellationToken)
             .ConfigureAwait(false);
    }

    public async ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string orderId,
        CancellationToken cancellationToken = default)
    {
        var request = new FundStatusRequest()
        {
            ApiUrl = apiUrl.ToString(),
            PartnerCode = partnerCode,
            PartnerKey = partnerKey,
            PartnerIv = partnerIV,
            OrderId = orderId
        };

        var reply = await _client
             .FundStatusAsync(
                 request: request,
                 cancellationToken: cancellationToken)
             .ConfigureAwait(false);

        return reply.Success;
    }

    public async IAsyncEnumerable<GameHistoryInfo> GameHistoryAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        bool isSettle,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameHistoryRequest()
        {
            ApiUrl = apiUrl.ToString(),
            PartnerCode = partnerCode,
            PartnerKey = partnerKey,
            PartnerIv = partnerIV,
            IsSettle = isSettle,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp()
        };
        var reply = await _client
             .GameHistoryAsync(
                 request: request,
                 cancellationToken: cancellationToken)
             .ConfigureAwait(false);

        if (!reply.Items.Any())
        {
            yield break;
        }

        foreach (var item in reply.Items)
        {
            yield return item;
        }
    }

    public async ValueTask<Uri> GameLoginAsync(
        Uri apiUrl,
        string partnerCode,
        string token,
        string lang,
        int version = 1,
        bool isPhone = false,
        CancellationToken cancellationToken = default)
    {
        var request = new GameLoginRequest()
        {
            ApiUrl = apiUrl.ToString(),
            PartnerCode = partnerCode,
            Token = token,
            Lang = lang,
            Version = version,
            IsPhone = isPhone,
        };

        var reply = await _client
             .GameLoginAsync(
                 request: request,
                 cancellationToken: cancellationToken)
             .ConfigureAwait(false);

        return new Uri(reply.GameUrl);
    }

    public async ValueTask<string> GamePrepareLoginAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string playerCode,
        string currencyCode,
        CancellationToken cancellationToken = default)
    {
        var request = new GamePrepareLoginRequest()
        {
            ApiUrl = apiUrl.ToString(),
            PartnerCode = partnerCode,
            PartnerKey = partnerKey,
            PartnerIv = partnerIV,
            PlayerCode = playerCode,
            CurrencyCode = currencyCode
        };

        var reply = await _client
             .GamePrepareLoginAsync(
                 request: request,
                 cancellationToken: cancellationToken)
             .ConfigureAwait(false);

        return reply.Token;
    }

    public async ValueTask<(bool IsPlayer, decimal Balance)> PlayerBalanceAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string playerCode,
        CancellationToken cancellationToken = default)
    {
        var request = new PlayerBalanceRequest()
        {
            ApiUrl = apiUrl.ToString(),
            PartnerCode = partnerCode,
            PartnerKey = partnerKey,
            PartnerIv = partnerIV,
            PlayerCode = playerCode,
        };

        var reply = await _client
              .PlayerBalanceAsync(
                  request: request,
                  cancellationToken: cancellationToken)
              .ConfigureAwait(false);

        return (reply.IsPlayer, reply.Balance);
    }

    public async ValueTask PlayerKickAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string playerCode,
        CancellationToken cancellationToken = default)
    {
        var request = new PlayerKickRequest()
        {
            ApiUrl = apiUrl.ToString(),
            PartnerCode = partnerCode,
            PartnerKey = partnerKey,
            PartnerIv = partnerIV,
            PlayerCode = playerCode,
        };

        _ = await _client
            .PlayerKickAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
