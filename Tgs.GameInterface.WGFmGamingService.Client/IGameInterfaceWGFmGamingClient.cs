using Tgs.GameInterface.WGFmGamingService.Protos;

namespace Tgs.GameInterface.WGFmGamingService.Client;

public interface IGameInterfaceWGFmGamingClient
{
    ValueTask FundInAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string playerCode,
        string orderId,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask FundOutAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string playerCode,
        string orderId,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string orderId,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameHistoryInfo> GameHistoryAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        bool isSettle,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        CancellationToken cancellationToken = default);

    ValueTask<Uri> GameLoginAsync(
        Uri apiUrl,
        string partnerCode,
        string token,
        string lang,
        int version = 1,
        CancellationToken cancellationToken = default);

    ValueTask<string> GamePrepareLoginAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string playerCode,
        string currencyCode,
        CancellationToken cancellationToken = default);

    ValueTask<(bool IsPlayer, decimal Balance)> PlayerBalanceAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string playerCode,
        CancellationToken cancellationToken = default);

    ValueTask PlayerKickLineAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string playerCode,
        CancellationToken cancellationToken = default);
}
