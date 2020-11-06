using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetFabric.Hyperlinq;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	sealed class EntityState<T> : IEntityState<T> where T : class
	{
		readonly DbContext                    _context;
		readonly Func<Predicate<EntityEntry>> _excluded;

		[UsedImplicitly]
		public EntityState(DbContext context) : this(context, Excluded.Default.Then().Bind(context)) {}

		public EntityState(DbContext context, Func<Predicate<EntityEntry>> excluded)
		{
			_context  = context;
			_excluded = excluded;
		}

		public async ValueTask<int> Get(T parameter)
		{
			var excluded = _excluded();

			_context.Set<T>().Update(parameter);

			var result = await _context.SaveChangesAsync().ConfigureAwait(false);

			foreach (var entry in _context.ChangeTracker.Entries().AsValueEnumerable().Where(excluded))
			{
				entry!.State = EntityState.Detached;
			}

			return result;
		}
	}
}