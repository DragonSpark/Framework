using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class New<TFrom, TTo> : IEntities<TFrom, TTo> where TFrom : class
{
	readonly IMapped _map;
	readonly Type    _to;

	public New(IMap map) : this(new Mapped(map), A.Type<TTo>()) {}

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
sealed class Update<TFrom, TTo> : IEntities<TFrom, TTo>
	where TFrom : class
	where TTo   : class
{
	readonly IMap _map;

	public Update(IMap map) => _map = map;

	public IQueryable<TTo> Get(ProcessChangesInput<TFrom> parameter)
	{
		var (_, _, source, destination, from, _) = parameter;

		var entityType = source.Model.FindEntityType(typeof(TFrom)).Verify();
		var key        = entityType.FindPrimaryKey().Verify();
		var name       = key.Properties.Single().Name;
		var projected = from.Select(x => new { Entity = x, Key = EF.Property<object>(x, name) });

		return Enumerate().AsQueryable();

		IEnumerable<TTo> Enumerate()
		{
			foreach (var row in projected)
			{
				var existing = destination.Set<TTo>().Single(y => EF.Property<object>(y, name) == row.Key);

				_map.Execute(new(source.Entry(row.Entity), destination.Entry(existing)));

				yield return existing;
			}
		}
	}
}

