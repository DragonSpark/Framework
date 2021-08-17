using DragonSpark.Application.Entities;
using DragonSpark.Runtime;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class MarkModified<T> : IMarkModified<T> where T : IdentityUser
	{
		readonly ITime    _time;
		readonly ISave<T> _save;

		public MarkModified(ISave<T> save) : this(save, Time.Default) {}

		public MarkModified(ISave<T> save, ITime time)
		{
			_save = save;
			_time = time;
		}

		public ValueTask Get(T parameter)
		{
			parameter.Modified = _time.Get();
			return _save.Get(parameter);
		}
	}
}