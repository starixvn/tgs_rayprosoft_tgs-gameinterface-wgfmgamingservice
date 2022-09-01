using Microsoft.Extensions.Options;
using Tgs.GameInterface.WGFmGamingService.Client;
using Tgs.GameInterface.WGFmGamingService.Protos;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameInterfaceSkywindServiceClient(
        this IServiceCollection services,
        Action<GameInterfaceWGFmGamingClientOptions, IServiceProvider> configure)
        => services
            .AddOptions<GameInterfaceWGFmGamingClientOptions>()
            .Configure(configure)
            .Services
            .AddGrpcClient<GameInterfaceWGFmGamingService.GameInterfaceWGFmGamingServiceClient>((sp, configureClient) =>
            {
                var options = sp.GetRequiredService<IOptions<GameInterfaceWGFmGamingClientOptions>>().Value;
                configureClient.Address = options.BaseUrl;
            })
            .ConfigureChannel((sp, configureChannel) =>
            {
                var handler = new SocketsHttpHandler
                {
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    EnableMultipleHttp2Connections = true
                };
                configureChannel.HttpHandler = handler;
            })
            .AddInterceptor<LoggingInterceptor>()
            .Services
            .AddSingleton<DiagnosticsInterceptorOptions>()
            .AddSingleton<LoggingInterceptor>()
            .AddTransient<IGameInterfaceWGFmGamingClient, GameInterfaceWGFmGamingClient>();
}
