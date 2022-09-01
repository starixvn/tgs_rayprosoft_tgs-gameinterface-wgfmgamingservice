namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_203_Exception : Exception
{
    public ReturnCode_203_Exception(string lang) : base($"無效的 Lang 類型({lang})")
    {
    }
}
