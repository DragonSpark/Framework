using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose.Deferred;

sealed class GetDeferredRegistrations : ISelect<IServiceCollection, DeferredRegistrations>
{
	public static GetDeferredRegistrations Default { get; } = new();

	GetDeferredRegistrations() : this(DeferredRegistrationStateAccessor.Default) { }

	readonly IDeferredRegistrationStateAccessor _accessor;

	public GetDeferredRegistrations(IDeferredRegistrationStateAccessor accessor) => _accessor = accessor;

	public DeferredRegistrations Get(IServiceCollection parameter)
		=> _accessor.Get(parameter.Context().Properties);
}