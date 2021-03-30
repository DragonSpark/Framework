using DragonSpark.Compose;
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

	sealed class SaveChanges<T> : ISaveChanges<T> where T : class
	{
		readonly DbContext _context;

		public SaveChanges(DbContext context) => _context = context;

		public async ValueTask<uint> Get(T parameter)
		{
			_context.Set<T>().Update(parameter);
			return (uint)await _context.SaveChangesAsync().ConfigureAwait(false);
		}
	}
}