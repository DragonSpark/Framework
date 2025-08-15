using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Construction;

sealed class WithComposition : IAlteration<IHostBuilder>
{
    readonly IResult<ServiceContainer> _services;
    public static WithComposition Default { get; } = new();

    WithComposition() : this(NewDefaultContainer.Default) {}

    public WithComposition(IResult<ServiceContainer> services) => _services = services;

    public IHostBuilder Get(IHostBuilder parameter)
    {
        var services = _services.Get();
        var @default = new LightInjectServiceProviderFactory(services);
        var factory  = new Factory(@default);
        var result   = parameter.UseServiceProviderFactory(factory);
        return result;
    }
}