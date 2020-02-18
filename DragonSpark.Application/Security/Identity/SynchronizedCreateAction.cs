using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class SynchronizedCreateAction : ICreateAction
	{
		readonly ICreateAction        _previous;
		readonly IUserSynchronization _synchronization;

		public SynchronizedCreateAction(ICreateAction previous, IUserSynchronization synchronization)
		{
			_previous        = previous;
			_synchronization = synchronization;
		}

		public async ValueTask<IdentityResult> Get(ExternalLoginInfo parameter)
		{
			var result = await _previous.Get(parameter);
			await _synchronization.Get(parameter);
			return result;
		}
	}
}