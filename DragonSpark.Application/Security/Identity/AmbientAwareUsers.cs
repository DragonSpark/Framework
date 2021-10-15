using DragonSpark.Composition.Scopes;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity;

sealed class AmbientAwareUsers<T> : IUsers<T> where T : class
{
	readonly IUsers<T>                   _previous;
	readonly IResult<AsyncServiceScope?> _established;

	public AmbientAwareUsers(IUsers<T> previous) : this(previous, LogicalScope.Default) {}

	public AmbientAwareUsers(IUsers<T> previous, IResult<AsyncServiceScope?> established)
	{
		_previous    = previous;
		_established = established;
	}

	public UsersSession<T> Get()
	{
		var current = _established.Get();
		var result = current != null
			             ? new(current.Value.ServiceProvider.GetRequiredService<UserManager<T>>())
			             : _previous.Get();
		return result;
	}
}