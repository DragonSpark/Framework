using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class HasValidState<T> : IHasValidState<T> where T : IdentityUser
	{
		readonly SignInManager<T> _authentication;
		readonly UserManager<T>   _users;

		public HasValidState(SignInManager<T> authentication) : this(authentication, authentication.UserManager) {}

		public HasValidState(SignInManager<T> authentication, UserManager<T> users)
		{
			_authentication = authentication;
			_users          = users;
		}

		public async ValueTask<bool> Get(T parameter)
		{
			var user = await _users.FindByNameAsync(parameter.UserName).ConfigureAwait(false);
			var result = await _authentication.ValidateSecurityStampAsync(user, parameter.SecurityStamp)
			                                  .ConfigureAwait(false);
			return result;
		}
	}
}