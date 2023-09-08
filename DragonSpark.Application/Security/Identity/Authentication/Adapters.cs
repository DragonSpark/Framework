using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
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

sealed class ExitAwareState<T> : ISelect<AuthenticationState<T>, AuthenticationState> where T : IdentityUser
{
	readonly ISelect<CurrentProfileStateInput, ProfileStatus> _state;
	readonly INavigateToSignOut                              _exit;
	readonly AuthenticationState                             _default;

	public ExitAwareState(INavigateToSignOut exit)
		: this(CurrentProfileState.Default, exit, AuthenticationState<T>.Default) {}

	public ExitAwareState(ISelect<CurrentProfileStateInput, ProfileStatus> state, INavigateToSignOut exit,
	                      AuthenticationState @default)
	{
		_state   = state;
		_exit    = exit;
		_default = @default;
	}

	public AuthenticationState Get(AuthenticationState<T> parameter)
	{
		var (user, _) = parameter;
		if (_state.Get(parameter) is ProfileStatus.Invalid)
		{
			_exit.Execute(user);
			return _default;
		}

		return parameter;
	}
}

public static class Extensions
{
	public static ProfileStatus Get<T>(this ISelect<CurrentProfileStateInput, ProfileStatus> @this,
	                                  AuthenticationState<T> parameter) where T : IdentityUser
		=> @this.Get(new(parameter.User, parameter.Profile));
}

public readonly record struct CurrentProfileStateInput(ClaimsPrincipal Principal, IdentityUser? User);

public sealed class CurrentProfileState : ISelect<CurrentProfileStateInput, ProfileStatus>
{
	public static CurrentProfileState Default { get; } = new();

	CurrentProfileState() : this(IdentityConstants.ApplicationScheme) {}

	readonly string _scheme;

	public CurrentProfileState(string scheme) => _scheme = scheme;

	public ProfileStatus Get(CurrentProfileStateInput parameter)
	{
		var (principal, user) = parameter;
		return principal.IsAuthenticated()
			       ? principal.Identity?.AuthenticationType == _scheme
				         ? user is not null
					           ? user.EmailConfirmed ? ProfileStatus.Confirmed : ProfileStatus.Confirming
					           : ProfileStatus.Invalid
				         : ProfileStatus.Authenticated
			       : ProfileStatus.Anonymous;
	}
}

public enum ProfileStatus
{
	Invalid,
	Anonymous,
	Authenticated,
	Confirming,
	Confirmed,

}