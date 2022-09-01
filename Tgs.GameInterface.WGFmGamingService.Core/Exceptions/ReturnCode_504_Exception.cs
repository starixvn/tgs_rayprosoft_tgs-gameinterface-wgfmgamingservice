namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_504_Exception : Exception
{
    public ReturnCode_504_Exception(string dateType) : base($"無效日期類型.({dateType})")
    {
    }
}
