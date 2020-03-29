using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class AuthenticationProfile : IAuthenticationProfile
	{
		readonly IAuthenticationProfile _profile;
		readonly IAppliedAuthentication _applied;

		public AuthenticationProfile(IAuthenticationProfile profile, IAppliedAuthentication applied)
		{
			_profile = profile;
			_applied = applied;
		}

		public async ValueTask<ExternalLoginInfo> Get()
		{
			var login  = await _profile.Get();
			var result = login != null ? _applied.Get(login) : null;
			return result;
		}
	}

	sealed class AuthenticationProfile<T> : IAuthenticationProfile where T : class
	{
		readonly SignInManager<T> _authentication;

		public AuthenticationProfile(SignInManager<T> authentication) => _authentication = authentication;

		public ValueTask<ExternalLoginInfo> Get() => _authentication.GetExternalLoginInfoAsync().ToOperation();
	}
}