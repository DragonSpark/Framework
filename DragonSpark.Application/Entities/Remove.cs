using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class Remove<TIn, T> : Operation<TIn>
	{
		public Remove(ISelecting<TIn, T> select, IRemove<T> remove) : base(select.Then().Terminate(remove)) {}
	}

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

		public async ValueTask Get(T parameter)
		{
			_set.Remove(parameter);
			await _save.Await();
		}
	}
}