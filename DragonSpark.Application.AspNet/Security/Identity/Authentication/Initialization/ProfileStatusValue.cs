using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class ProfileStatusValue : IResult<ProfileStatus?>
{
	readonly IProfileStateInput                               _state;
	readonly ISelect<CurrentProfileStateInput, ProfileStatus> _status;

	public ProfileStatusValue(IProfileStateInput state) : this(state, GetProfileStatus.Default) {}

	public ProfileStatusValue(IProfileStateInput state, ISelect<CurrentProfileStateInput, ProfileStatus> status)
	{
		_state  = state;
		_status = status;
	}

	public ProfileStatus? Get() => _state.Get() is {} state ? _status.Get(state) : null;
}