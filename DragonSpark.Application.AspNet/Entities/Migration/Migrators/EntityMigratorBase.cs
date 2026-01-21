using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public class EntityMigratorBase<TFrom, TTo> : Instance<EntityTypeMapping>, IEntityMigrator
	where TFrom : class where TTo : class
{
	readonly Batching<TFrom> _batching;
	readonly IBatch<TFrom>   _batch;

	protected EntityMigratorBase(DbContext source, DbContext destination)
		: this(new(source, destination), Map.Default) {}

	protected EntityMigratorBase(Batching<TFrom> batching, IMap map)
		: this(batching, Batches<TFrom, TTo>.Default.Get(new(batching.Source, batching.Destination, map))) {}

	protected EntityMigratorBase(Batching<TFrom> batching, IBatch<TFrom> batch) : base(new(typeof(TFrom), typeof(TTo)))
	{
		_batching = batching;
		_batch    = batch;
	}

	public void Execute(EntityMigratorInput parameter)
	{
		var (logger, size)                 = parameter;
		var (source, destination, subject) = _batching;
		var total = subject.Count().Grade();
		if (total > 0)
		{
			for (var offset = 0; offset < total; offset += size)
			{
				_batch.Execute(new(logger, source, destination, subject, new(offset, size), total));
			}

			source.ChangeTracker.Clear();
			destination.ChangeTracker.Clear();
		}
		else
		{
			logger.LogInformation("{From} -> {To}: No rows found in source", A.Type<TFrom>(), A.Type<TTo>());
		}
	}

	public void Execute(EntityPreMigrationInput parameter) {}

	public void Execute(EntityPostMigrationInput parameter) {}
}