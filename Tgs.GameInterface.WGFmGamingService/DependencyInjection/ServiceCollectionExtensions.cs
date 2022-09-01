namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameInterfaceService(this IServiceCollection services)
        => services
            .AddGameInterfaceCore();
}
