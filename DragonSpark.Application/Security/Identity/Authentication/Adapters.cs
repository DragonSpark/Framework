using DragonSpark.Compose;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class Adapters<T> : IAdapters where T : IdentityUser
{
	readonly IStateViews<T>    _views;
	readonly ExitAwareState<T> _state;

	public Adapters(IStateViews<T> views, ExitAwareState<T> state)
	{
		_views = views;
		_state = state;
	}

	public async Task<AuthenticationState> Get(Task<AuthenticationState> parameter)
	{
		var previous = await parameter.ConfigureAwait(false);
		var user     = previous.User;
		var (state, _) = await _views.Await(user);
		var result = _state.Get(state);
		return result;
	}
}