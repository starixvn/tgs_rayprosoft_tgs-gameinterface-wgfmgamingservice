namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_105_Exception : Exception
{
    public ReturnCode_105_Exception() : base("無效的 hash, 注意:hash 當 url 參數時,需要加 urlencode()")
    {
    }
}
