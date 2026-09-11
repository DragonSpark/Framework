using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class EntityMap<TFrom, TTo> : IEntityMap<TFrom, TTo> where TFrom : class where TTo : class
{
	readonly IOriginAware              _workspaces;
	readonly Func<EntityEntry, object> _keys;
	readonly IEqualityComparer<object> _comparer;

	public EntityMap(IOriginAware workspaces)
		: this(workspaces, Keys.Default.Get, StructuralEqualityComparer.Default) {}

	public EntityMap(IOriginAware workspaces, Func<EntityEntry, object> keys, IEqualityComparer<object> comparer)
	{
		_workspaces = workspaces;
		_keys       = keys;
		_comparer   = comparer;
	}

	public async ValueTask<IPopAware<object, Migrators.Instances.Entry<TTo>>> Get(
		Stop<IReadOnlyCollection<TFrom>> parameter)
	{
		var (subject, stop) = parameter;
		await using var workspace = _workspaces.Origin();
		var (source, destination) = workspace;
		var to   = destination.Set<TTo>();
		var keys = subject.Select(Start.A.Selection<object, EntityEntry>(source.Entry).Select(_keys).Get).Result();

		const int batchSize = 250; // TODO
		var       store     = new ConcurrentDictionary<object, Migrators.Instances.Entry<TTo>>(_comparer);

		foreach (var chunk in keys.Open().Chunk(batchSize))
		{
			var where    = new WhereKeysExist<TTo>(chunk).Get(to.EntityType);
			var existing = await to.Where(where).ToArrayAsync(stop).Off();

			foreach (var x in existing)
			{
				var key = _keys(to.Entry(x));
				store[key] = new Migrators.Instances.Entry<TTo>(x, to.Entry(x).CurrentValues);
			}
		}

		return new ConcurrentTable<object, Migrators.Instances.Entry<TTo>>(store);
	}
}