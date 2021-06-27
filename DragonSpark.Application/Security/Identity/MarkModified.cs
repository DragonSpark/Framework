using DragonSpark.Application.Entities;
using DragonSpark.Runtime;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class MarkModified<T> : IMarkModified<T> where T : IdentityUser
	{
		readonly ITime           _time;
		readonly ISaveChanges<T> _save;

		public MarkModified(ISaveChanges<T> save) : this(save, Time.Default) {}

		public MarkModified(ISaveChanges<T> save, ITime time)
		{
			_time = time;
			_save = save;
		}

		public ValueTask<uint> Get(T parameter)
		{
			parameter.Modified = _time.Get();
			return _save.Get(parameter);
		}
	}
}