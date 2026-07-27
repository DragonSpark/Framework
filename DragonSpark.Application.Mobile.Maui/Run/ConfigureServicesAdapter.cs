using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui.Run;

sealed class ConfigureServicesAdapter : ICommand<MauiAppBuilder>
{
    readonly ICommand<IServiceCollection> _previous;

    public ConfigureServicesAdapter(ICommand<IServiceCollection> previous) => _previous = previous;

    public void Execute(MauiAppBuilder parameter)
    {
        _previous.Execute(parameter.Services);
    }
}