using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Collections.Concurrent;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class BuildStore<TFrom, TTo> : IBuildStore<TFrom, TTo> where TFrom : class where TTo : class
{
	public static BuildStore<TFrom,TTo> Default { get; } = new();

	BuildStore() : this(Keys.Default.Get, StructuralEqualityComparer.Default, BatchSize.Default) {}

	readonly Func<EntityEntry, object> _keys;
	readonly IEqualityComparer<object> _comparer;
	readonly ushort                    _size;

	public BuildStore(Func<EntityEntry, object> keys, IEqualityComparer<object> comparer, ushort size)
	{
		_keys     = keys;
		_comparer = comparer;
		_size     = size;
	}

	public async ValueTask<IDictionary<object, Migrators.Instances.Entry<TTo>>> Get(Stop<BuildStoreInput<TFrom>> parameter)
	{
		var (((from, destination), page), stop) = parameter;
		var       to     = destination.Set<TTo>();
		using var keys   = page.AsValueEnumerable().Select(from.Entry).Select(_keys).ToArray(ArrayPool<object>.Shared);
		var       result = new ConcurrentDictionary<object, Migrators.Instances.Entry<TTo>>(_comparer);

		foreach (var chunk in keys.Open().Chunk(_size))
		{
			var where    = new WhereKeysExist<TTo>(chunk).Get(to.EntityType);
			var existing = await to.Where(where).ToArrayAsync(stop).Off();

			foreach (var x in existing)
			{
				var key = _keys(to.Entry(x));
				result[key] = new Migrators.Instances.Entry<TTo>(x, to.Entry(x).CurrentValues);
			}
		}

		return result;
	}
}