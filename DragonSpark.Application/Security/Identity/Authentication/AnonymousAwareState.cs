using DragonSpark.Compose;
using JetBrains.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class AnonymousAwareState<T> : IStateViews<T> where T : IdentityUser
{
	readonly IStateViews<T> _previous;
	readonly StateView<T>   _default;

	[UsedImplicitly]
	public AnonymousAwareState(IStateViews<T> previous) : this(previous, StateView<T>.Default) {}

	public AnonymousAwareState(IStateViews<T> previous, StateView<T> @default)
	{
		_previous = previous;
		_default  = @default;
	}

	public ValueTask<StateView<T>> Get(ClaimsPrincipal parameter)
		=> parameter.Identity?.IsAuthenticated ?? false ? _previous.Get(parameter) : _default.ToOperation();
}