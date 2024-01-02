namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_204_Exception(string version) : Exception($"無效的 Version 參數({version})")
{
}
