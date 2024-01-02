namespace Tgs.GameInterface.WGFmGamingService.Core.Exceptions;

public class ReturnCode_396_Exception(string extTrxId) : Exception($"賽事剛派彩還沒有放款(沒有放款時，只能存，不能取).({extTrxId})")
{
}
