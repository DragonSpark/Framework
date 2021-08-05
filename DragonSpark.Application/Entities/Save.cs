using DragonSpark.Compose;
using DragonSpark.Composition;
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

	sealed class Save<T> : ISave<T> where T : class
	{
		readonly DbContext                     _context;
		readonly ISaveChanges<T>               _save;
		readonly Func<Func<EntityEntry, bool>> _excluded;

		[UsedImplicitly]
		public Save(DbContext context, ISaveChanges<T> save)
			: this(context, save, Excluded.Default.Then().Bind(context)) {}

		[Candidate(false)]
		public Save(DbContext context, ISaveChanges<T> save, Func<Func<EntityEntry, bool>> excluded)
		{
			_context  = context;
			_save     = save;
			_excluded = excluded;
		}

		public async ValueTask Get(T parameter)
		{
			var excluded = _excluded();

			await _save.Await(parameter);

			foreach (var entry in _context.ChangeTracker.Entries().AsValueEnumerable().Where(excluded))
			{
				entry!.State = EntityState.Detached;
			}
		}
	}
}