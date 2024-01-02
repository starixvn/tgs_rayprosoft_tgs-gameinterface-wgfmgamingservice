namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_504_Exception(string dateType) : Exception($"無效日期類型.({dateType})")
{
}
