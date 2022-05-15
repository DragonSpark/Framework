using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Compose.Deferred;

sealed class AddDeferredRegistrations : ICommand<IHostBuilder>
{
	public static AddDeferredRegistrations Default { get; } = new();

	AddDeferredRegistrations() : this(DeferredRegistrationStateAccessor.Default) { }

	readonly IDeferredRegistrationStateAccessor _accessor;

	public AddDeferredRegistrations(IDeferredRegistrationStateAccessor accessor) => _accessor = accessor;

	public void Execute(IHostBuilder parameter)
	{
		var registrations = new DeferredRegistrations();
		_accessor.Assign(parameter.Properties, registrations);
	}
}