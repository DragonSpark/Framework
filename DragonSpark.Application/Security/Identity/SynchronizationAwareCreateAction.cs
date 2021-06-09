using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	// TODO: Register
	sealed class SynchronizationAwareNew<T> : INew<T> where T : IdentityUser
	{
		readonly INew<T>              _previous;
		readonly IUserSynchronization _synchronization;

		public SynchronizationAwareNew(INew<T> previous, IUserSynchronization synchronization)
		{
			_previous        = previous;
			_synchronization = synchronization;
		}

		public async ValueTask<T> Get(ExternalLoginInfo parameter)
		{
			var result = await _previous.Get(parameter);
			await _synchronization.Get(parameter);
			return result;
		}
	}
}