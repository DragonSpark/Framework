using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

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

	public async ValueTask Get(Stop<ExternalLoginInfo> parameter)
	{
		var (subject, stop) = parameter;
		var user    = await _locate.Off(new(subject, stop));
		var changed = await _synchronizer.Off(new(parameter, user.Verify()));
		if (changed)
		{
			_clear.Execute(subject.Principal);
		}
	}
}