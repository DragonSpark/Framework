using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	sealed class Save : ISave
	{
		readonly DbContext _storage;

		public Save(DbContext storage) => _storage = storage;

		public ValueTask<int> Get() => _storage.SaveChangesAsync().ToOperation();
	}

	sealed class Save<T> : ISave<T> where T : class
	{
		readonly DbSet<T> _set;
		readonly Operate  _save;

		public Save(DbContext context) : this(context.Set<T>(), new Save(context)) {}

		public Save(DbSet<T> set, ISave save) : this(set, save.Then().Terminate()) {}

		public Save(DbSet<T> set, Operate save)
		{
			_set  = set;
			_save = save;
		}

		public ValueTask Get(T parameter)
		{
			_set.Update(parameter);
			return _save();
		}
	}

	public class Save<TContext, T> : IOperation<T> where TContext : DbContext where T : class
	{
		readonly IDbContextFactory<TContext> _context;

		public Save(IDbContextFactory<TContext> context) => _context = context;

		public async ValueTask Get(T parameter)
		{
			await using var context = _context.CreateDbContext();
			context.Set<T>().Update(parameter);
			await context.SaveChangesAsync();
		}
	}
}