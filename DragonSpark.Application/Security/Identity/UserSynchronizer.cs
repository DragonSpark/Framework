using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	class UserSynchronizer<T> : IUserSynchronizer<T> where T : IdentityUser
	{
		readonly IUserSynchronizer<T> _previous;
		readonly IMarkModified<T>     _modified;

		public UserSynchronizer(UserClaimSynchronizer<T> previous, IMarkModified<T> modified)
		{
			_previous = previous;
			_modified = modified;
		}

		public async ValueTask<bool> Get(Synchronization<T> parameter)
		{
			var result = await _previous.Get(parameter).ConfigureAwait(false);
			if (result)
			{
				await _modified.Get(parameter.Profile.User).ConfigureAwait(false);
			}
			return result;
		}
	}

	public interface IMarkModified<in T> : IOperationResult<T, int> where T : IdentityUser {}

	sealed class MarkModified<T> : IMarkModified<T> where T : IdentityUser
	{
		readonly DbContext _context;
		readonly ITime     _time;

		public MarkModified(DbContext context) : this(context, Time.Default) {}

		public MarkModified(DbContext context, ITime time)
		{
			_context = context;
			_time    = time;
		}

		public ValueTask<int> Get(T parameter)
		{
			parameter.Modified = _time.Get();
			var result = _context.SaveChangesAsync().ToOperation();
			return result;
		}
	}
}