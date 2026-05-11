using DragonSpark.Application.AspNet.Entities.Migration.Steps;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class FlattenAwareEntityMigrator<T> : IEntityMigrator where T : class
{
	readonly IEntityMigrator             _previous;
	readonly DbContext                   _destination;
	readonly IStopAware<DbContext, bool> _first;

	public FlattenAwareEntityMigrator(IEntityMigrator previous, DbContext destination)
		: this(previous, destination, FirstRun.Default) {}

	public FlattenAwareEntityMigrator(IEntityMigrator previous, DbContext destination,
	                                  IStopAware<DbContext, bool> first)
	{
		_previous    = previous;
		_destination = destination;
		_first       = first;
	}

	public EntityTypeMapping Get() => _previous.Get();

	public async ValueTask Get(Stop<EntityPreMigrationInput> parameter)
	{
		var (subject, stop) = parameter;
		var logger = subject.Logger;
		var to     = _destination.Set<T>();
		if (await _first.Off(new(_destination, stop)))
		{
			var cleared = await to.ExecuteDeleteAsync(stop).Off();
			logger.LogInformation("Flatten {Set}: Cleared of {Count} entries", to.GetType(), cleared);
		}
	}

	public ValueTask Get(Stop<EntityPostMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityMigratorInput> parameter) => _previous.Get(parameter);
}