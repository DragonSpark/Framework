using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public sealed class EntityMigrator<TFrom, TTo> : EntityMigratorBase<TFrom, TTo> where TFrom : class where TTo : class
{
	public EntityMigrator(DbContext source, DbContext destination) : this(source, destination, Map.Default) {}

	public EntityMigrator(DbContext source, DbContext destination, IMap map) : this(new(source, destination), map) {}

	public EntityMigrator(Contexts<TFrom> contexts, IMap map) : base(contexts, map) {}

	public EntityMigrator(Contexts<TFrom> contexts, IEntityProcessor<TFrom> processor) : base(contexts, processor) {}
}

sealed class NamedEntityMigrator : EntityMigratorBase<Dictionary<string, object>, Dictionary<string, object>>
{
	public NamedEntityMigrator(Contexts<Dictionary<string, object>> contexts, IEntityType type)
		: base(contexts, new NamedEntityProcessor(type)) {}
}

// TODO V2

sealed class NamedEntityProcessor : EntityProcessorBase<Dictionary<string, object>, Dictionary<string, object>>
{
	public NamedEntityProcessor(IEntityType type)
		: base(Source<Dictionary<string, object>>.Default, new NamedDestination(type),
		       Save<Dictionary<string, object>>.Default) {}
}

sealed class NamedDestination : IDestination<Dictionary<string, object>, Dictionary<string, object>>
{
	readonly IEntityType                  _type;
	readonly IStopAware<DbContext, bool?> _run;

	public NamedDestination(IEntityType type) : this(type, MigrationHasRun.Default) {}

	public NamedDestination(IEntityType type, IStopAware<DbContext, bool?> run)
	{
		_type = type;
		_run  = run;
	}

	public async IAsyncEnumerable<Dictionary<string, object>> Get(
		Stop<DestinationInput<Dictionary<string, object>>> parameter)
	{
		var ((_, _, destination, from, _), stop) = parameter;

		var run = await _run.Off(new(destination, stop));
		if (run is not null && !run.Value)
		{
			foreach (var item in from.Open())
			{
				var columns    = string.Join(", ", item.Keys.Select(k => $"[{k}]"));
				var parameters = string.Join(", ", item.Keys.Select((k, i) => $"@p{i}"));
				var values     = item.Values.ToArray();

				var schema = _type.GetSchema();
				var sql =
					$"INSERT INTO {(schema.IsAssigned() ? $"[{schema}]." : string.Empty)}[{_type.GetTableName()}] ({columns}) VALUES ({parameters})";

				await destination.Database.ExecuteSqlRawAsync(sql, values, stop).Off();
			}
		}

		yield break;
	}
}