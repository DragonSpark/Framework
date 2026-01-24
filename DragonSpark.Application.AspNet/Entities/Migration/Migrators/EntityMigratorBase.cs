using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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

	public void Execute(EntityMigratorInput parameter)
	{
		var (logger, size)                    = parameter;
		var (source, destination, _, subject) = _contexts;
		var total = subject.Count().Grade();
		_processor.Execute(new(logger, size, source, destination, subject, total));
	}

	public void Execute(EntityPreMigrationInput parameter) {}

	public void Execute(EntityPostMigrationInput parameter) {}
}