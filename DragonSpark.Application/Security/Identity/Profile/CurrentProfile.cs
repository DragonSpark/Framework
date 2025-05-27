using System.Threading.Tasks;
using DragonSpark.Compose;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class CurrentProfile : ICurrentProfile
{
	readonly ICurrentPrincipal _principal;
	readonly ICreateProfile    _profile;

	public CurrentProfile(ICurrentPrincipal principal, ICreateProfile profile)
	{
		_principal = principal;
		_profile   = profile;
	}

	public ValueTask<Profile> Get()
	{
		var principal     = _principal.Get();
		var authenticated = principal.IsAuthenticated();
		var profile       = authenticated ? _profile.Get(principal).To<Profile>() : DefaultProfile.Default;
		var result        = profile.ToOperation();
		return result;
	}
}