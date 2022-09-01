namespace Tgs.GameInterface.WGFmGamingService.ViewModels;

public record GameHistoryResultViewModel(
    string PlayerCode,
    string ExternalRoundId,
    string ExternalGameId,
    string ExternalCreatedOn,
    string ExternalClosedOn,
    bool IsClosed,
    bool IsVoid,
    decimal Bet,
    decimal ValidBet,
    decimal PlayerWinLoss,
    decimal Turnover,
    decimal Settle,
    string AdditionalData);
