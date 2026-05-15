using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Construction;

sealed class ComposeWithComposition : IAlteration<IHostBuilder>
{
    public static ComposeWithComposition Default { get; } = new();

    ComposeWithComposition() : this(NewDefaultContainer.Default) {}

    readonly IResult<ServiceContainer> _services;

    public ComposeWithComposition(IResult<ServiceContainer> services) => _services = services;

    public IHostBuilder Get(IHostBuilder parameter)
    {
        var services = _services.Get();
        var @default = new LightInjectServiceProviderFactory(services);
        var factory  = new Factory(@default);
        var result   = parameter.UseServiceProviderFactory(factory);
        return result;
    }
}