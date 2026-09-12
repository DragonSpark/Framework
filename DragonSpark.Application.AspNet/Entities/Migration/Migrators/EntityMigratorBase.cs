using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public class EntityMigratorBase<TFrom, TTo> : Instance<EntityTypeMapping>, IEntityMigrator
	where TFrom : class where TTo : class
{
	readonly Func<DbContext, IQueryable<TFrom>> _query;
	readonly IEntityProcessor<TFrom>            _processor;

	protected EntityMigratorBase(DbContext source, IModel destination) : this(new(source, destination), Map.Default) {}

	protected EntityMigratorBase(DbContext source, IModel destination,
	                             Func<Stop<MapInput<TFrom, TTo>>, ValueTask> map)
		: this(new(source, destination), new Map<TFrom, TTo>(map)) {}

	protected EntityMigratorBase(DbContext source, IModel destination, Action<MapInput<TFrom, TTo>> map)
		: this(new(source, destination), new Map<TFrom, TTo>(map)) {}

	protected EntityMigratorBase(DbContext source, IModel destination, Action<TFrom, TTo> map)
		: this(new(source, destination), new Map<TFrom, TTo>(map)) {}

	protected EntityMigratorBase(Contexts<TFrom> contexts, IMap map)
		: this(contexts.Query, Processors<TFrom, TTo>.Default.Get(new(contexts, map))) {}

	protected EntityMigratorBase(Func<DbContext, IQueryable<TFrom>> query, IEntityProcessor<TFrom> processor)
		: base(new(typeof(TFrom), typeof(TTo)))
	{
		_query = query;
		_processor   = processor;
	}

	public ValueTask Get(Stop<EntityPreMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityPostMigrationInput> parameter) => ValueTask.CompletedTask;

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var ((logger, definition, size), stop) = parameter;
		await using var workspace = definition.Get();
		var (origin, _) = workspace;
		try
		{
			var query    = _query(origin);
			var total    = await query.CountAsync().Off();
			var enhanced = new Enhanced<TFrom>(definition, origin);
			var entities = new Workspaces.Entities(definition, origin, enhanced);
			await _processor.Off(new(new(logger, entities, query, size, total.Grade()), stop));
		}
		catch (Exception e)
		{
			logger.LogError(e, "A problem was encountered while processing the entities {From} -> {To}", typeof(TFrom),
			                typeof(TTo));
			throw;
		}
	}
}