using DragonSpark.Model.Selection;
using LightInject;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Construction;

public sealed class HostFactory : IHostFactory
{
    public static HostFactory Default { get; } = new();

    HostFactory() : this(NewFactories.Default) {}

    readonly ISelect<IDictionary<object, object>, IServiceProviderFactory<IServiceContainer>> _new;

    public HostFactory(ISelect<IDictionary<object, object>, IServiceProviderFactory<IServiceContainer>> @new)
        => _new = @new;

    public IServiceProviderFactory<IServiceContainer> Get(IHostBuilder parameter) => _new.Get(parameter.Properties);
}