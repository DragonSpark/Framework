using DragonSpark.Compose;
using System;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class New<TFrom, TTo> : IEntities<TFrom, TTo> where TFrom : class
{
	readonly IMapped _map;
	readonly Type    _to;

	public New(IMap map) : this(new New(map), A.Type<TTo>()) {}

	public New(IMapped map, Type to)
	{
		_map = map;
		_to  = to;
	}

	public IQueryable<TTo> Get(ProcessChangesInput<TFrom> parameter)
	{
		var (_, _, source, destination, from, _) = parameter;
		var result = from.Select(x => (TTo)_map.Get(new(source, destination, x, _to)));
		return result;
	}
}

public sealed class New : IMapped
{
	public static New Default { get; } = new();

	New() : this(Map.Default) {}

	readonly Func<Type, object> _new;
	readonly IMap               _map;

	public New(IMap map) : this(A.New, map) {}

	public New(Func<Type, object> @new, IMap map)
	{
		_new = @new;
		_map = map;
	}

	public object Get(MappingInput parameter)
	{
		var (source, destination, from, to) = parameter;
		var result = _new(to);
		_map.Execute(new(source.Entry(from), destination.Entry(result)));
		return result;
	}
}