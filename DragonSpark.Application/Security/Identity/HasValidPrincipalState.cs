using DragonSpark.Application.Entities;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class HasValidPrincipalState<T> : IHasValidPrincipalState where T : class
	{
		readonly SignInManager<T> _authentication;
		readonly IClear           _clear;

		public HasValidPrincipalState(SignInManager<T> authentication, IClear clear)
		{
			_authentication = authentication;
			_clear          = clear;
		}

		public async ValueTask<bool> Get(ClaimsPrincipal parameter)
		{
			_clear.Execute();
			var user   = await _authentication.ValidateSecurityStampAsync(parameter).ConfigureAwait(false);
			var result = user != null;
			return result;
		}
	}
}