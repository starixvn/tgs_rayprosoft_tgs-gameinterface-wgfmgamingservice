using Tgs.GameInterface.WGFmGamingService.Core.Models;

namespace Tgs.GameInterface.WGFmGamingService.Core;

public interface IGameApiClient
{
    ValueTask AccessMoneyWithdrawalAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string playerCode,
        string orderId,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask AccessMoneyDepositAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string playerCode,
        string orderId,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<bool> AccessMoneyQueryAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string orderId,
        CancellationToken cancellationToken = default);

    ValueTask<(bool IsPlayer, decimal Balance)> GetBalanceAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string playerCode,
        CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<ShowBetInfo>> ShowBetListAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        ShowBetListDateType dateType,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        CancellationToken cancellationToken = default);

    ValueTask<Uri> LoginAsync(
        Uri apiUrl,
        string partnerCode,
        string token,
        string lang,
        int version = 1,
        int IsPhone = 0,
        CancellationToken cancellationToken = default);

    ValueTask<string> PrepareLoginAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        long timestamp,
        string playerCode,
        string playerNickname,
        string currencyCode,
        CancellationToken cancellationToken = default);

    ValueTask LogoutAsync(
        Uri apiUrl,
        string partnerCode,
        string partnerKey,
        string partnerIV,
        string playerCode,
        CancellationToken cancellationToken = default);
}
