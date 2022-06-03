using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class Adapters<T> : IAdapters where T : IdentityUser
{
	readonly IStateViews<T>      _views;
	readonly INavigateToSignOut  _exit;
	readonly AuthenticationState _default;

	[UsedImplicitly]
	public Adapters(IStateViews<T> views, INavigateToSignOut exit)
		: this(views, exit, AuthenticationState<T>.Default) {}

	public Adapters(IStateViews<T> views, INavigateToSignOut exit, AuthenticationState @default)
	{
		_views   = views;
		_exit    = exit;
		_default = @default;
	}

	public async Task<AuthenticationState> Get(Task<AuthenticationState> parameter)
	{
		var previous = await parameter.ConfigureAwait(false);
		var (result, _) = await _views.Await(previous.User);

		if ((previous.User.Identity?.IsAuthenticated ?? false) && result.Profile == null)
		{
			_exit.Execute(previous.User);
			return _default;
		}

		return result;
	}
}