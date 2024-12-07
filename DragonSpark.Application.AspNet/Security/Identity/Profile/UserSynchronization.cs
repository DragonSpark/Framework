using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class UserSynchronization<T> : IUserSynchronization where T : IdentityUser
{
	readonly ILocateUser<T>             _locate;
	readonly IUserSynchronizer<T>       _synchronizer;
	readonly ClearCurrentAuthentication _clear;

	public UserSynchronization(ILocateUser<T> locate, IUserSynchronizer<T> synchronizer,
	                           ClearCurrentAuthentication clear)
	{
		_locate       = locate;
		_synchronizer = synchronizer;
		_clear        = clear;
	}

	public async ValueTask Get(ExternalLoginInfo parameter)
	{
		var user    = await _locate.Await(parameter);
		var changed = await _synchronizer.Await(new(parameter, user.Verify()));
		if (changed)
		{
			_clear.Execute(parameter.Principal);
		}
	}
}