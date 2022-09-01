namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_103_Exception : Exception
{
    public ReturnCode_103_Exception() : base("貨幣未正確,只能是 HKD 或者 RMB")
    {
    }
}
