namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_203_Exception(string lang) : Exception($"無效的 Lang 類型({lang})")
{
}
