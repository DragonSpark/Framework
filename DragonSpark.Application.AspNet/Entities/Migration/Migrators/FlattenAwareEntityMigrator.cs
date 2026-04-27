using DragonSpark.Application.AspNet.Entities.Migration.Identity;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

	public EntityTypeMapping Get() => _previous.Get();


	public async ValueTask Get(Stop<EntityPreMigrationInput> parameter)
	{
		var (subject, stop) = parameter;
		var logger       = subject.Logger;
		var to           = _destination.Set<TTo>();
		var exists       = KnownKeys<TFrom>.Default.Get(_source).IsSubsetOf(KnownKeys<TTo>.Default.Get(_destination));
		if (exists)
		{
			logger.LogInformation("Flatten {Set}: All source keys already present in destination (idempotent, no missing data)",
			                      to.GetType());
		}
		else
		{
			var cleared = await to.ExecuteDeleteAsync(stop).Off();
			logger.LogInformation("Flatten {Set}: Cleared of {Count} entries", to.GetType(), cleared);
			await _previous.Off(parameter);
		}
	}

	public ValueTask Get(Stop<EntityPostMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityMigratorInput> parameter) => ValueTask.CompletedTask;
}