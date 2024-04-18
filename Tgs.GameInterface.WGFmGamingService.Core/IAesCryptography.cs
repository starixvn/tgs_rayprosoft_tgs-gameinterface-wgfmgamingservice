namespace Tgs.GameInterface.WGFmGamingService.Core;

public interface IAesCryptography
{
	ValueTask<string> GetAesEncryptAsync(
		string data,
		string secretKey,
		string secretIV,
		CancellationToken cancellationToken = default);
}
