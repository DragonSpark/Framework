using System.Threading.Tasks;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Uno.Security.Identity.Profile;

sealed class ProfileInformation : IProfileInformation
{
	readonly ICurrentPrincipal _principal;
	readonly ICreateProfile    _profile;

	public ProfileInformation(ICurrentPrincipal principal, ICreateProfile profile)
	{
		_principal = principal;
		_profile   = profile;
	}

	public ValueTask<Profile> Get()
	{
		var principal     = _principal.Get();
		var authenticated = principal.IsAuthenticated();
		var profile       = authenticated ? _profile.Get(principal) : DefaultProfile.Default;
		var result        = profile.ToOperation();
		return result;
	}
}