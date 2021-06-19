
using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile
{
	sealed class UserSynchronization<T> : IUserSynchronization where T : IdentityUser
	{
		readonly IRefreshAuthentication<T> _refresh;
		readonly ILocateUser<T>            _locate;
		readonly IUserSynchronizer<T>      _synchronizer;

		public UserSynchronization(IRefreshAuthentication<T> refresh, ILocateUser<T> locate,
		                           IUserSynchronizer<T> synchronizer)
		{
			_refresh      = refresh;
			_locate       = locate;
			_synchronizer = synchronizer;
		}

		public async ValueTask Get(ExternalLoginInfo parameter)
		{
			var locate = await _locate.Await(parameter);
			var user   = locate.Verify();
			if (await _synchronizer.Await(new(parameter, user)))
			{
				await _refresh.Await(user);
			}
		}
	}
}