using DragonSpark.Application.Security.Identity.Claims.Compile;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	sealed class Authenticate<T> : IAuthenticate<T> where T : IdentityUser
	{
		readonly SignInManager<T> _authentication;
		readonly IClaims          _claims;
		readonly bool             _persist;

		public Authenticate(SignInManager<T> authentication, IClaims claims, bool persist = true)
		{
			_authentication = authentication;
			_claims         = claims;
			_persist        = persist;
		}

		public async ValueTask Get(Login<T> parameter)
		{
			var (information, user) = parameter;
			var claims = _claims.Get(new(information.Principal, information.LoginProvider, information.ProviderKey));
			await _authentication.SignInWithClaimsAsync(user, _persist, claims).ConfigureAwait(false);
		}
	}
}