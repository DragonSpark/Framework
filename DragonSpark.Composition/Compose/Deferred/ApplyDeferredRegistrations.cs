using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose.Deferred;

sealed class ApplyDeferredRegistrations : ICommand<IServiceCollection>
{
	public static ApplyDeferredRegistrations Default { get; } = new();

	ApplyDeferredRegistrations() : this(GetDeferredRegistrations.Default) { }

	readonly ISelect<IServiceCollection, DeferredRegistrations> _accessor;

	public ApplyDeferredRegistrations(ISelect<IServiceCollection, DeferredRegistrations> accessor)
		=> _accessor = accessor;

	public void Execute(IServiceCollection parameter)
	{
		foreach (var registration in _accessor.Get(parameter))
		{
			registration.Execute(parameter);
		}
	}
}