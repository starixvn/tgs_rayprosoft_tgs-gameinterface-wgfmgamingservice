namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_104_Exception : Exception
{
    public ReturnCode_104_Exception(string PartnerCode) : base($"錯誤的合作公司標誌,即無效的 PartnerCode({PartnerCode})")
    {
    }
}
