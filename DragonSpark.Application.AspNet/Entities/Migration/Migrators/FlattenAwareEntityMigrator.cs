using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class FlattenAwareEntityMigrator<TFrom, TTo> : IEntityMigrator where TFrom : class where TTo : class
{
	readonly IEntityMigrator _previous;
	readonly DbContext       _source, _destination;

	public FlattenAwareEntityMigrator(IEntityMigrator previous, DbContext source, DbContext destination)
	{
		_previous    = previous;
		_source      = source;
		_destination = destination;
	}

	public void Execute(EntityPreMigrationInput parameter)
	{
		var logger = parameter.Logger;
		var to     = _destination.Set<TTo>();
		var exists = KnownKeys<TFrom>.Default.Get(_source).IsSubsetOf(KnownKeys<TTo>.Default.Get(_destination));
		if (exists)
		{
			logger.LogInformation("Flatten {Set}: All source keys already present in destination (idempotent, no missing data)",
			                      to.GetType());
		}
		else
		{
			var cleared = to.ExecuteDelete();
			logger.LogInformation("Flatten {Set}: Cleared of {Count} entries", to.GetType(), cleared);
			_previous.Execute(parameter);
		}

	}

	public void Execute(EntityMigratorInput parameter)
	{
	}

	public EntityTypeMapping Get() => _previous.Get();

	public void Execute(EntityPostMigrationInput parameter) {}
}