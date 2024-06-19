using System.Diagnostics;

namespace Tgs.GameInterface.WGFmGamingService;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } = new ActivitySource(typeof(DiagnosticsInterceptorOptions).FullName!);
}
