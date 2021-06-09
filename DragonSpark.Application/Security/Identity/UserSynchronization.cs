using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class UserSynchronization<T> : IUserSynchronization where T : IdentityUser
	{
		readonly SignInManager<T>     _authentication;
		readonly ILocateUser<T>       _locate;
		readonly IUserSynchronizer<T> _synchronizer;

		public UserSynchronization(SignInManager<T> authentication, ILocateUser<T> locate,
		                           IUserSynchronizer<T> synchronizer)
		{
			_authentication = authentication;
			_locate         = locate;
			_synchronizer   = synchronizer;
		}

		public async ValueTask Get(ExternalLoginInfo parameter)
		{
			var user = await _locate.Await(parameter);

			if (await _synchronizer.Get(new(parameter, user)))
			{
				await _authentication.RefreshSignInAsync(user);
			}
		}
	}
}