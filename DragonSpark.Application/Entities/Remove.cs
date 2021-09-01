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
		readonly DbContext _context;
		readonly DbSet<T>  _set;

		public Remove(DbContext context) : this(context, context.Set<T>()) {}

		public Remove(DbContext context, DbSet<T> @set)
		{
			_context = context;
			_set     = set;
		}

		public async ValueTask Get(T parameter)
		{
			_set.Remove(parameter);
			await _context.SaveChangesAsync().ConfigureAwait(false);
		}
	}
}