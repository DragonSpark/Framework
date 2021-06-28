using DragonSpark.Application.Entities;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class HasValidState<T> : IHasValidState<T> where T : IdentityUser
	{
		readonly SignInManager<T> _authentication;
		readonly UserManager<T>   _users;
		readonly IClear           _clear;

		public HasValidState(SignInManager<T> authentication, IClear clear)
			: this(authentication, authentication.UserManager, clear) {}

		public HasValidState(SignInManager<T> authentication, UserManager<T> users, IClear clear)
		{
			_authentication = authentication;
			_users          = users;
			_clear          = clear;
		}

		public async ValueTask<bool> Get(T parameter)
		{
			_clear.Execute();
			var user = await _users.FindByNameAsync(parameter.UserName).ConfigureAwait(false);
			var result = await _authentication.ValidateSecurityStampAsync(user, parameter.SecurityStamp)
			                                  .ConfigureAwait(false);
			return result;
		}
	}
}