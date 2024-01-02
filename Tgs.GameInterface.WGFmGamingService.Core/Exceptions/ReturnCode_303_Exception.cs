namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_303_Exception(decimal amount) : Exception($"無效金額({amount})")
{
}
