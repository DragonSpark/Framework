using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components.Authorization;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class ExitAwareState<T> : ISelect<AuthenticationState<T>, AuthenticationState> where T : IdentityUser
{
	readonly ISelect<CurrentProfileStateInput, ProfileStatus> _state;
	readonly SignOutCurrentPath                               _exit;
	readonly AuthenticationState                              _default;

	public ExitAwareState(SignOutCurrentPath exit)
		: this(GetProfileStatus.Default, exit, AuthenticationState<T>.Default) {}

	public ExitAwareState(ISelect<CurrentProfileStateInput, ProfileStatus> state, SignOutCurrentPath exit,
	                      AuthenticationState @default)
	{
		_state   = state;
		_exit    = exit;
		_default = @default;
	}

	public AuthenticationState Get(AuthenticationState<T> parameter)
	{
		if (_state.Get(parameter) is ProfileStatus.Invalid)
		{
			_exit.Execute();
			return _default;
		}

		return parameter;
	}
}