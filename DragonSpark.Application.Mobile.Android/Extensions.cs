using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Android;

public static class Extensions
{
    public static IServiceCollection RegisterFrameworkAndroidServices(this IServiceCollection @this)
        => Registrations.Default.Parameter(@this);
}