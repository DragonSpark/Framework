using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Run;

sealed class ConfigureApplicationAdapter : ICommand<IServiceCollection>
{
    readonly ICommand<MauiAppBuilder> _previous;

    public ConfigureApplicationAdapter(ICommand<MauiAppBuilder> previous) => _previous = previous;

    public void Execute(IServiceCollection parameter)
    {
        _previous.Execute(parameter.Application());
    }
}