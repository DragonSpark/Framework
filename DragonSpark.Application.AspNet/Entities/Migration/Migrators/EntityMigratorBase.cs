using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public class EntityMigratorBase<TFrom, TTo> : Instance<EntityTypeMapping>, IEntityMigrator
	where TFrom : class where TTo : class
{
	readonly Batching<TFrom>         _batching;
	readonly IEntityProcessor<TFrom> _processor;

	protected EntityMigratorBase(DbContext source, DbContext destination)
		: this(new(source, destination), Map.Default) {}

	protected EntityMigratorBase(Batching<TFrom> batching, IMap map)
		: this(batching, Processors<TFrom, TTo>.Default.Get(new(batching.Source, batching.Destination, map))) {}

	protected EntityMigratorBase(Batching<TFrom> batching, IEntityProcessor<TFrom> processor)
		: base(new(typeof(TFrom), typeof(TTo)))
	{
		_batching  = batching;
		_processor = processor;
	}

	public void Execute(EntityMigratorInput parameter)
	{
		var (logger, size)                 = parameter;
		var (source, destination, subject) = _batching;
		var total = subject.Count().Grade();
		_processor.Execute(new(logger, size, source, destination, subject, total));
	}

	public void Execute(EntityPreMigrationInput parameter) {}

	public void Execute(EntityPostMigrationInput parameter) {}
}