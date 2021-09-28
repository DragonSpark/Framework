using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing
{
	sealed class SessionSave<T> : ISessionSave<T> where T : class
	{
		readonly DbContext _context;
		readonly DbSet<T>  _set;

		public SessionSave(DbContext context) : this(context, context.Set<T>()) {}

		public SessionSave(DbContext context, DbSet<T> set)
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
}