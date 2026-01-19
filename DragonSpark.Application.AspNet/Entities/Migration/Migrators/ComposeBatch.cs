using DragonSpark.Compose;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class ComposeBatch<TFrom, TTo> : IComposeBatch<TFrom, TTo> where TFrom : class
{
	readonly IMapped        _map;
	readonly Type           _to;
	readonly ArrayPool<TTo> _pool;

	public ComposeBatch(IMap map) : this(new Mapped(map), A.Type<TTo>(), ArrayPool<TTo>.Shared) {}

	public ComposeBatch(IMapped map, Type to, ArrayPool<TTo> pool)
	{
		_map  = map;
		_to   = to;
		_pool = pool;
	}

	public Lease<TTo> Get(BatchInput<TFrom> parameter)
	{
		var (_, source, destination, from, (skip, top), _) = parameter;
		var offset = skip.Value();
		var result = from.Skip(offset)
		                 .Take(top.Value())
		                 .Select(x => (TTo)_map.Get(new(source, destination, x, _to)))
		                 .AsValueEnumerable()
		                 .ToArray(_pool);
		return result;
	}
}