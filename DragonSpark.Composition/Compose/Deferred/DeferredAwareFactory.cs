using DragonSpark.Model.Commands;
using LightInject;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose.Deferred;

sealed class DeferredAwareFactory : IServiceProviderFactory<IServiceContainer>
{
    readonly IServiceProviderFactory<IServiceContainer> _previous;
    readonly ICommand<IServiceCollection>               _command;

    public DeferredAwareFactory(IServiceProviderFactory<IServiceContainer> previous)
        : this(previous, ApplyDeferredRegistrations.Default) {}

    public DeferredAwareFactory(IServiceProviderFactory<IServiceContainer> previous,
                                ICommand<IServiceCollection> command)
    {
        _previous = previous;
        _command  = command;
    }

    public IServiceContainer CreateBuilder(IServiceCollection services)
    {
        _command.Execute(services);
        return _previous.CreateBuilder(services);
    }

    public IServiceProvider CreateServiceProvider(IServiceContainer containerBuilder)
        => _previous.CreateServiceProvider(containerBuilder);
}