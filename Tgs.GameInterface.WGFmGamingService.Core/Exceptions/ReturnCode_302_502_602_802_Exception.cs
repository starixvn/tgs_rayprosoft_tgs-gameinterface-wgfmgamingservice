namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_302_502_602_802_Exception : Exception
{
	public ReturnCode_302_502_602_802_Exception() : base($"無效用戶")
	{
	}

	public ReturnCode_302_502_602_802_Exception(string playerCode) : base($"無效用戶({playerCode})")
	{
	}
}
