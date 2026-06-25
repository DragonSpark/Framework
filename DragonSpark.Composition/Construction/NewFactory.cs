using DragonSpark.Model.Results;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Construction;

sealed class NewFactory : IResult<IServiceProviderFactory<IServiceContainer>>
{
    public static NewFactory Default { get; } = new();

    NewFactory() : this(NewDefaultContainer.Default) {}

    readonly IResult<ServiceContainer> _new;

    public NewFactory(IResult<ServiceContainer> @new) => _new = @new;

    public IServiceProviderFactory<IServiceContainer> Get()
    {
        var @new    = _new.Get();
        var start   = new LightInjectServiceProviderFactory(@new);
        var factory = new Factory(start);
        return new AssignableFactory(factory);
    }
}