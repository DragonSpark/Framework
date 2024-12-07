using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class SynchronizationAwareCreateExternal<T> : ICreateExternal<T> where T : IdentityUser
{
	readonly ICreateExternal<T>   _previous;
	readonly IUserSynchronization _synchronization;

	public SynchronizationAwareCreateExternal(ICreateExternal<T> previous, IUserSynchronization synchronization)
	{
		_previous        = previous;
		_synchronization = synchronization;
	}

	public async ValueTask<CreateUserResult<T>> Get(ExternalLoginInfo parameter)
	{
		var result = await _previous.Await(parameter);
		await _synchronization.Await(parameter);
		return result;
	}
}