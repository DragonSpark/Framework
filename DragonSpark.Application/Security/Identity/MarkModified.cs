using DragonSpark.Application.Entities.Editing;
using DragonSpark.Compose;
using DragonSpark.Runtime;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class MarkModified<T> : IMarkModified<T> where T : IdentityUser
	{
		readonly ITime   _time;
		readonly Save<T> _save;

		public MarkModified(Save<T> save) : this(save, Time.Default) {}

		public MarkModified(Save<T> save, ITime time)
		{
			_save = save;
			_time = time;
		}

		public async ValueTask Get(T parameter)
		{
			parameter.Modified = _time.Get();
			await _save.Await(parameter);
		}
	}
}