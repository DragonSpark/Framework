using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class UserSynchronization<T> : IUserSynchronization where T : IdentityUser
	{
		readonly SignInManager<T>     _authentication;
		readonly IUserSynchronizer<T> _synchronizer;
		readonly UserManager<T>       _users;

		public UserSynchronization(SignInManager<T> authentication, UserManager<T> users,
		                           IUserSynchronizer<T> synchronizer)
		{
			_authentication = authentication;
			_synchronizer   = synchronizer;
			_users          = users;
		}

		public async ValueTask Get(ExternalLoginInfo parameter)
		{
			var user      = await _users.GetUserAsync(parameter.Principal);
			var principal = await _authentication.CreateUserPrincipalAsync(user);
			var synchronization = new Synchronization<T>(parameter, new AuthenticationState<T>(principal, user),
			                                             parameter.Principal);
			if (await _synchronizer.Get(synchronization))
			{
				await _authentication.RefreshSignInAsync(user);
			}
		}
	}
}