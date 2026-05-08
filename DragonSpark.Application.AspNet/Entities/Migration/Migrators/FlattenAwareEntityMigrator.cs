using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class FlattenAwareEntityMigrator<TFrom, TTo> : IEntityMigrator where TFrom : class where TTo : class
{
	readonly IEntityMigrator _previous;
	readonly DbContext       _destination;
	readonly bool            _same;

	public FlattenAwareEntityMigrator(IEntityMigrator previous, DbContext source, DbContext destination)
		: this(previous, destination, SameKeys<TFrom, TTo>.Default.Get(new(source, destination))) {}

	public FlattenAwareEntityMigrator(IEntityMigrator previous, DbContext destination, bool same)
	{
		_previous    = previous;
		_destination = destination;
		_same        = same;
	}

	public EntityTypeMapping Get() => _previous.Get();

	public async ValueTask Get(Stop<EntityPreMigrationInput> parameter)
	{
		var (subject, stop) = parameter;
		var logger = subject.Logger;
		var to     = _destination.Set<TTo>();
		if (_same)
		{
			logger.LogInformation("Flatten {Set}: All source keys already present in destination (idempotent, no missing data)",
			                      to.GetType());
		}
		else
		{
			var cleared = await to.ExecuteDeleteAsync(stop).Off();
			logger.LogInformation("Flatten {Set}: Cleared of {Count} entries", to.GetType(), cleared);
		}
	}

	public ValueTask Get(Stop<EntityPostMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityMigratorInput> parameter)
		=> _same ? ValueTask.CompletedTask : _previous.Get(parameter);
}