using DragonSpark.Model.Selection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Hosting.Maui.Run;

sealed class InitializeBuilder(Func<IHostBuilder, IHostBuilder> host, Action<MauiAppBuilder> configure)
    : ISelect<MauiAppBuilder, MauiAppBuilder>
{
    public MauiAppBuilder Get(MauiAppBuilder parameter)
    {
        parameter.Services.AddSingleton(parameter);
        var builder = new MauiHostBuilder(parameter);
        host(builder);
        configure(parameter);
        parameter.Services.Remove(parameter.Services.Single(x => x.ImplementationInstance == parameter));
        return parameter;
    }
}