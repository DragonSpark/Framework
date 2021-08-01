using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile
{
	sealed class SynchronizationAwareCreated<T> : ICreated<T> where T : IdentityUser
	{
		readonly ICreated<T>          _previous;
		readonly IUserSynchronization _synchronization;

		public SynchronizationAwareCreated(ICreated<T> previous, IUserSynchronization synchronization)
		{
			_previous        = previous;
			_synchronization = synchronization;
		}

		public async ValueTask<IdentityResult> Get(Login<T> parameter)
		{
			var (information, _) = parameter;
			var result = await _previous.Await(parameter);
			await _synchronization.Await(information);
			return result;
		}
	}
}