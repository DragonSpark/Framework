using DragonSpark.Compose;
using DragonSpark.Composition.Construction;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Compose.Deferred;

sealed class AddDeferredRegistrations : ICommand<IHostBuilder>
{
    public static AddDeferredRegistrations Default { get; } = new();

    AddDeferredRegistrations() : this(DeferredRegistrationStateAccessor.Default, HostFactory.Default) {}

    readonly IDeferredRegistrationStateAccessor _accessor;
    readonly IHostFactory                       _factory;

    public AddDeferredRegistrations(IDeferredRegistrationStateAccessor accessor, IHostFactory factory)
    {
        _accessor = accessor;
        _factory  = factory;
    }

    public void Execute(IHostBuilder parameter)
    {
        var registrations = new DeferredRegistrations();
        _accessor.Assign(parameter.Properties, registrations);

        var factory = _factory.Get(parameter) as IAssignableFactory ??
                      throw new InvalidOperationException("The host factory must implement IAssignableFactory");
        factory.Execute(new DeferredAwareFactory(factory.Get()));
    }
}