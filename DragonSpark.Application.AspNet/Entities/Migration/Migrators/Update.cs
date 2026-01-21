using DragonSpark.Compose;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

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
		var projected  = from.Select(x => new { Entity = x, Key = EF.Property<object>(x, name) });

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

sealed class Update<T> : ISave<T> where T : class
{
	public static Update<T> Default { get; } = new();

	Update() {}

	public uint Get(SaveInput<T> parameter)
	{
		var (logger, size, destination, entities, total) = parameter;
		var configuration = new BulkConfig { BatchSize = size, CalculateStats = true, NotifyAfter = size };
		destination.BulkUpdate(entities, configuration, new Progress<T>(logger, total).Execute);
		var result = configuration.StatsInfo.Verify().StatsNumberUpdated.Grade();
		return result;
	}
}