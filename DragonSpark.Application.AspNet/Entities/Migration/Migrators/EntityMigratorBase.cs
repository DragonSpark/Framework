using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public class EntityMigratorBase<TFrom, TTo> : Instance<EntityTypeMapping>, IEntityMigrator
	where TFrom : class where TTo : class
{
	readonly IStopAware<EntityMigratorInput> _body;

	protected EntityMigratorBase(DbContext source, IModel destination) : this(new(source, destination), Map.Default) {}

	protected EntityMigratorBase(DbContext source, IModel destination, Func<Stop<MapInput<TFrom, TTo>>, ValueTask> map)
		: this(new(source, destination), new Map<TFrom, TTo>(map)) {}

	protected EntityMigratorBase(DbContext source, IModel destination, Action<MapInput<TFrom, TTo>> map)
		: this(new(source, destination), new Map<TFrom, TTo>(map)) {}

	protected EntityMigratorBase(DbContext source, IModel destination, Action<TFrom, TTo> map)
		: this(new(source, destination), new Map<TFrom, TTo>(map)) {}

	protected EntityMigratorBase(Contexts<TFrom> contexts, IMap map)
		: this(contexts.Query, Processors<TFrom, TTo>.Default.Get(new(contexts, map))) {}

	protected EntityMigratorBase(Func<DbContext, IQueryable<TFrom>> query, IEntityProcessor<TFrom> processor)
		: this(ComposeMigrate<TFrom, TTo>.Default.Get(new(query, processor))) {}

	protected EntityMigratorBase(IStopAware<EntityMigratorInput> body) : base(new(typeof(TFrom), typeof(TTo)))
		=> _body = body;

	public ValueTask Get(Stop<EntityPreMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityPostMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityMigratorInput> parameter) => _body.Get(parameter);
}