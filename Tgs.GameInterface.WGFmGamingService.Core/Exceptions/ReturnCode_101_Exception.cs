namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_101_Exception : Exception
{
    public ReturnCode_101_Exception() : base("請求過期,原因是當前時間與解密 Hash 後得到的時間間隔太久.")
    {
    }
}
