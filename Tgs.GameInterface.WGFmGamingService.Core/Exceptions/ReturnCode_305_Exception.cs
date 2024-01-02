namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_305_Exception(string extTrxId) : Exception($"訂單號重複.({extTrxId})")
{
}
