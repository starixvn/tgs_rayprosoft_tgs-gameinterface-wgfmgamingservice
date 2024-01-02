namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_Unknown_Exception(int returnCode) : Exception($"遊戲供應商未提供錯誤代碼{returnCode}")
{
}
