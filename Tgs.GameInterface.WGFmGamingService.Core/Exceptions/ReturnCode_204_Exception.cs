namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_204_Exception : Exception
{
    public ReturnCode_204_Exception(string version) : base($"無效的 Version 參數({version})")
    {
    }
}
