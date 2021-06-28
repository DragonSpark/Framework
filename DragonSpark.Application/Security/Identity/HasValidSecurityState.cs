using DragonSpark.Application.Entities;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	public sealed class HasValidSecurityState<T> : IHasValidSecurityState where T : class
	{
		readonly SignInManager<T> _authentication;
		readonly IClear           _clear;

		public HasValidSecurityState(SignInManager<T> authentication, IClear clear)
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