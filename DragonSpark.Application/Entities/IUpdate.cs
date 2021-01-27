using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public interface IUpdate<in T> : ISelecting<T, uint> {}

	public interface IRemove<in T> : ISelecting<T, uint> {}

	sealed class Remove<T> : IRemove<T> where T : class
	{
		readonly DbSet<T> _set;
		readonly ISave    _save;

		public Remove(DbContext context, ISave save) : this(context.Set<T>(), save) {}

		public Remove(DbSet<T> @set, ISave save)
		{
			_set  = set;
			_save = save;
		}

		public async ValueTask<uint> Get(T parameter)
		{
			_set.Remove(parameter);
			return (uint)await _save.Await();
		}
	}
}