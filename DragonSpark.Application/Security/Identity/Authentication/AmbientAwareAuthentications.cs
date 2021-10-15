using DragonSpark.Composition.Scopes;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class AmbientAwareAuthentications<T> : IAuthentications<T> where T : class
{
	readonly IAuthentications<T>         _previous;
	readonly IResult<AsyncServiceScope?> _established;

	public AmbientAwareAuthentications(IAuthentications<T> previous) : this(previous, LogicalScope.Default) {}

	public AmbientAwareAuthentications(IAuthentications<T> previous, IResult<AsyncServiceScope?> established)
	{
		_previous    = previous;
		_established = established;
	}

	public AuthenticationSession<T> Get()
	{
		var current = _established.Get();
		var result = current != null
			             ? new(current.Value.ServiceProvider.GetRequiredService<SignInManager<T>>())
			             : _previous.Get();
		return result;
	}
}