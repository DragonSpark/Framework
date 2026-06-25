using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using LightInject;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Construction;

sealed class FactoryConfiguration : IAlteration<IHostBuilder>
{
    public static FactoryConfiguration Default { get; } = new();

    FactoryConfiguration() : this(HostFactory.Default) {}

    readonly ISelect<IHostBuilder, IServiceProviderFactory<IServiceContainer>> _factory;

    public FactoryConfiguration(ISelect<IHostBuilder, IServiceProviderFactory<IServiceContainer>> factory)
        => _factory = factory;

    public IHostBuilder Get(IHostBuilder parameter)
    {
        var factory = _factory.Get(parameter);
        var result  = parameter.UseServiceProviderFactory(factory);
        return result;
    }
}