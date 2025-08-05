using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Run;
public static class Extensions
{
    public static ICommand<MauiAppBuilder> Adapt(this ICommand<IServiceCollection> @this)
        => new ConfigureServicesAdapter(@this);

    public static MauiAppBuilder Application(this IServiceCollection @this)
        => @this.GetRequiredInstance<MauiAppBuilder>();
    public static ICommand<IServiceCollection> Adapt(this ICommand<MauiAppBuilder> @this)
        => new ConfigureApplicationAdapter(@this);
}