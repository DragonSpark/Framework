using DragonSpark.Compose;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	[UsedImplicitly]
	sealed class UserSynchronizer<T> : IUserSynchronizer<T> where T : IdentityUser
	{
		/*readonly IUserSynchronizer<T> _previous;
		readonly IMarkModified<T>     _modified;

		public UserSynchronizer(UserClaimSynchronizer<T> previous, IMarkModified<T> modified)
		{
			_previous = previous;
			_modified = modified;
		}

		public async ValueTask<bool> Get(Synchronization<T> parameter)
		{
			var result = await _previous.Await(parameter);
			if (result)
			{
				var (_, (user, _), _) = parameter;

				await _modified.Get(user.Verify()).ConfigureAwait(false);
			}
			return result;
		}*/
		public ValueTask<bool> Get(Synchronization<T> parameter) => false.ToOperation();
	}
}