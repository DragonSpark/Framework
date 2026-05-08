using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class New<TFrom, TTo> : IEntities<TFrom, TTo> where TFrom : class
{
	readonly IInstance _instance;
	readonly Type      _to;

	public New(IMap map) : this(new New(map), A.Type<TTo>()) {}

	public New(IInstance instance, Type to)
	{
		_instance = instance;
		_to       = to;
	}

	public IQueryable<TTo> Get(Stop<ProcessChangesInput<TFrom>> parameter)
	{
		var ((_, _, source, destination, from, _), stop) = parameter;

		return new Entities<TTo>(EnumerateAsync());

		async IAsyncEnumerable<TTo> EnumerateAsync()
		{
			foreach (var x in from)
			{
				yield return (TTo)await _instance.Off(new(new(source, destination, x, _to), stop));
			}
		}
	}
}

public sealed class New : IInstance
{
	public static New Default { get; } = new();

	New() : this(DefaultMap.Default) {}

	readonly Func<Type, object> _new;
	readonly IMap               _map;

	public New(IMap map) : this(A.New, map) {}

	public New(Func<Type, object> @new, IMap map)
	{
		_new = @new;
		_map = map;
	}

	public async ValueTask<object> Get(Stop<MappingInput> parameter)
	{
		var ((source, destination, from, to), stop) = parameter;

		var result = _new(to);
		await _map.Off(new(new(source.Entry(from), destination.Entry(result)), stop));

		return result;
	}
}