using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class ProcessAuthentication<T> : IProcessAuthentication<T> where T : IdentityUser
{
	readonly AuthenticationStateSource<T>                     _state;
	readonly ProfileStatusSource                              _profile;
	readonly ISelect<CurrentProfileStateInput, ProfileStatus> _status;

	public ProcessAuthentication(AuthenticationStateSource<T> state, ProfileStatusSource profile)
		: this(state, profile, GetProfileStatus.Default) {}

	public ProcessAuthentication(AuthenticationStateSource<T> state, ProfileStatusSource profile,
	                             ISelect<CurrentProfileStateInput, ProfileStatus> status)
	{
		_state   = state;
		_profile = profile;
		_status  = status;
	}

	public async ValueTask Get(AuthenticationState<T> parameter)
	{
		await _state.NotifyChangedAsync(parameter).Off();
		var status = _status.Get(new(parameter.User, parameter.Profile));
		await _profile.NotifyChangedAsync(status).Off();
	}
}