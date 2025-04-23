using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Uno.Platforms.iOS;

public static class Extensions
{
    public static IServiceCollection RegisterFrameworkServices(this IServiceCollection @this)
        => Registrations.Default.Parameter(@this);
}