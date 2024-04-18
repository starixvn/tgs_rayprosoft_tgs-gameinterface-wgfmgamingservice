using System.Diagnostics;

namespace Tgs.GameInterface.WGFmGamingService.Client;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.WGFmGamingService.Client.DiagnosticsInterceptor");
}
