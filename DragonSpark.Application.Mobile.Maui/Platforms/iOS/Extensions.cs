using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS;

public static class Extensions
{
    public static IServiceCollection RegisterFrameworkServices(this IServiceCollection @this)
        => Registrations.Default.Parameter(@this);

    public static IServiceCollection WithHttp(this IServiceCollection @this)
        => Http.Registrations.Default.Parameter(@this);

    public static IServiceCollection WithAttestation(this IServiceCollection @this)
        => Attestation.Registrations.Default.Parameter(@this);
}