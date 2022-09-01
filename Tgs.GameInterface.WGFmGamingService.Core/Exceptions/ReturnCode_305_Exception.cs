namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_305_Exception : Exception
{
    public ReturnCode_305_Exception(string extTrxId) : base($"訂單號重複.({extTrxId})")
    {
    }
}
