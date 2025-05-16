using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android;

public static class Extensions
{
    public static IServiceCollection RegisterFrameworkServices(this IServiceCollection @this)
        => Registrations.Default.Parameter(@this);
    public static IServiceCollection WithHttp(this IServiceCollection @this)
        => Http.Registrations.Default.Parameter(@this);

}