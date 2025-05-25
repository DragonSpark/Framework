using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

sealed class SynchronizationAwareCreateExternal<T> : ICreateExternal<T> where T : IdentityUser
{
	readonly ICreateExternal<T>   _previous;
	readonly IUserSynchronization _synchronization;

	public SynchronizationAwareCreateExternal(ICreateExternal<T> previous, IUserSynchronization synchronization)
	{
		_previous        = previous;
		_synchronization = synchronization;
	}

	public async ValueTask<CreateUserResult<T>> Get(Stop<ExternalLoginInfo> parameter)
	{
		var result = await _previous.Off(parameter);
		await _synchronization.Off(parameter);
		return result;
	}
}