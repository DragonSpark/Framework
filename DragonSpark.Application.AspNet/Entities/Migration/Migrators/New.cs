using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

	public IQueryable<TTo> Get(Stop<ProcessChangesInput<TFrom>> parameter)
	{
		var ((_, _, source, destination, from, _), stop) = parameter;

		var query = from.AsAsyncEnumerable();

		return new Entities<TTo>(EnumerateAsync());

		async IAsyncEnumerable<TTo> EnumerateAsync()
		{
			await foreach (var x in query.WithCancellation(stop))
			{
				yield return (TTo)await _map.Off(new(new(source, destination, x, _to), stop));
			}
		}
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

	public async ValueTask<object> Get(Stop<MappingInput> parameter)
	{
		var ((source, destination, from, to), stop) = parameter;
		var result = _new(to);
		try
		{
			await _map.Off(new(new(source.Entry(from), destination.Entry(result)), stop));
		}
		catch (Exception e)
		{
			throw;
		}
		return result;
	}
}