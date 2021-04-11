using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	sealed class SaveChanges<T> : ISaveChanges<T> where T : class
	{
		readonly DbContext  _context;
		readonly IUpdate<T> _update;

		public SaveChanges(DbContext context, IUpdate<T> update)
		{
			_context = context;
			_update  = update;
		}

		public async ValueTask<uint> Get(T parameter)
		{
			_update.Execute(parameter);
			return (uint)await _context.SaveChangesAsync().ConfigureAwait(false);
		}
	}
}