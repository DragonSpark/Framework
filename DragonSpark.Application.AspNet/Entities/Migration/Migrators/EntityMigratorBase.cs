using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public class EntityMigratorBase<TFrom, TTo> : Instance<EntityTypeMapping>, IEntityMigrator
	where TFrom : class where TTo : class
{
	readonly Contexts<TFrom>         _contexts;
	readonly IEntityProcessor<TFrom> _processor;

	protected EntityMigratorBase(DbContext source, DbContext destination)
		: this(new(source, destination), Map.Default) {}

	protected EntityMigratorBase(DbContext source, DbContext destination, Action<MapInput<TFrom, TTo>> map)
		: this(new(source, destination), new Map<TFrom, TTo>(map)) {}

	protected EntityMigratorBase(DbContext source, DbContext destination, Action<TFrom, TTo> map)
		: this(new(source, destination), new Map<TFrom, TTo>(map)) {}

	protected EntityMigratorBase(Contexts<TFrom> contexts, IMap map)
		: this(contexts,
		       Processors<TFrom, TTo>.Default.Get(new(contexts.Source, contexts.Destination, contexts.From, map))) {}

	protected EntityMigratorBase(Contexts<TFrom> contexts, IEntityProcessor<TFrom> processor)
		: base(new(typeof(TFrom), typeof(TTo)))
	{
		_contexts  = contexts;
		_processor = processor;
	}

	public ValueTask Get(Stop<EntityPreMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityPostMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var ((logger, size), stop)            = parameter;
		var (source, destination, _, subject) = _contexts;
		var total = subject.Count().Grade();
		return _processor.Get(new(new(logger, size, source, destination, subject, total), stop));
	}
}