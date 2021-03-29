using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetFabric.Hyperlinq;
using System;
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
		readonly DbContext                    _context;
		readonly Func<Predicate<EntityEntry>> _excluded;

		[UsedImplicitly]
		public SaveChanges(DbContext context) : this(context, Excluded.Default.Then().Bind(context)) {}

		public SaveChanges(DbContext context, Func<Predicate<EntityEntry>> excluded)
		{
			_context  = context;
			_excluded = excluded;
		}

		public async ValueTask<uint> Get(T parameter)
		{
			var excluded = _excluded();

			_context.Set<T>().Update(parameter);

			var result = (uint)await _context.SaveChangesAsync().ConfigureAwait(false);

			foreach (var entry in _context.ChangeTracker.Entries().AsValueEnumerable().Where(excluded))
			{
				entry!.State = EntityState.Detached;
			}

			return result;
		}
	}
}