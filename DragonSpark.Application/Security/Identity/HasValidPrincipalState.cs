using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class HasValidPrincipalState<T> : IHasValidPrincipalState where T : class
	{
		readonly SignInManager<T> _authentication;

		public HasValidPrincipalState(SignInManager<T> authentication) => _authentication = authentication;

		public async ValueTask<bool> Get(ClaimsPrincipal parameter)
		{
			var user   = await _authentication.ValidateSecurityStampAsync(parameter).ConfigureAwait(false);
			var result = user != null;
			return result;
		}
	}
}