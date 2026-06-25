using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose.Deferred;

sealed class ApplyDeferredRegistrations : ICommand<IServiceCollection>
{
	public static ApplyDeferredRegistrations Default { get; } = new();

	ApplyDeferredRegistrations() : this(GetDeferredRegistrations.Default) { }

	readonly ISelect<IServiceCollection, DeferredRegistrations?> _accessor;

	public ApplyDeferredRegistrations(ISelect<IServiceCollection, DeferredRegistrations?> accessor)
		=> _accessor = accessor;

	public void Execute(IServiceCollection parameter)
	{
		var registrations = _accessor.Get(parameter);
		if (registrations is not null)
		{
			foreach (var registration in registrations)
			{
				registration.Execute(parameter);
			}
		}
	}
}