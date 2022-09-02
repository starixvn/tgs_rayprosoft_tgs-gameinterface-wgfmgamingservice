namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_303_Exception : Exception
{
    public ReturnCode_303_Exception(decimal amount) : base($"無效金額({amount})")
    {
    }
}
