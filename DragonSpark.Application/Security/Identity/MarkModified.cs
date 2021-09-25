using DragonSpark.Application.Entities.Editing;
using DragonSpark.Runtime;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class MarkModified<TContext, T> : IMarkModified<T> 
		where TContext : DbContext
		where T : IdentityUser
	{
		readonly ITime             _time;
		readonly Save<TContext, T> _save;

		public MarkModified(Save<TContext, T> save) : this(save, Time.Default) {}

		public MarkModified(Save<TContext, T> save, ITime time)
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