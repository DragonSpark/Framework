using DragonSpark.Runtime;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	class UserSynchronizer<T> : IUserSynchronizer<T> where T : IdentityUser
	{
		readonly IUserSynchronizer<T> _previous;
		readonly ITime                _time;

		public UserSynchronizer(UserClaimSynchronizer<T> previous) : this(previous, Time.Default) {}

		public UserSynchronizer(UserClaimSynchronizer<T> previous, ITime time)
		{
			_previous = previous;
			_time     = time;
		}

		public async ValueTask<bool> Get(Synchronization<T> parameter)
		{
			var result = await _previous.Get(parameter);
			if (result)
			{
				parameter.Stored.User.Modified = _time.Get();
			}

			return result;
		}
	}
}