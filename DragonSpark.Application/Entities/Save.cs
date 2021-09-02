using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	sealed class Save<T> : ISave<T> where T : class
	{
		readonly DbContext _context;
		readonly DbSet<T>  _set;

		public Save(DbContext context) : this(context, context.Set<T>()) {}

		public Save(DbContext context, DbSet<T> set)
		{
			_context = context;
			_set     = set;
		}

		public async ValueTask Get(T parameter)
		{
			_set.Update(parameter);
			await _context.SaveChangesAsync().ConfigureAwait(false);
		}
	}

	public class Save<TContext, T> : Modify<TContext, T> where TContext : DbContext where T : class
	{
		public Save(ISaveContext<TContext> save) : base(save, x => x.Context.Set<T>().Update(x.Parameter)) {}
	}
}