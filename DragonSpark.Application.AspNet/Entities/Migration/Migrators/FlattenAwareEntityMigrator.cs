using DragonSpark.Application.AspNet.Entities.Migration.Steps;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class FlattenAwareEntityMigrator<T> : IEntityMigrator where T : class
{
	readonly IEntityMigrator             _previous;
	readonly IStopAware<DbContext, bool> _first;

	public FlattenAwareEntityMigrator(IEntityMigrator previous) : this(previous, FirstRun.Default) {}

	public FlattenAwareEntityMigrator(IEntityMigrator previous, IStopAware<DbContext, bool> first)
	{
		_previous = previous;
		_first    = first;
	}

	public EntityTypeMapping Get() => _previous.Get();

	public async ValueTask Get(Stop<EntityPreMigrationInput> parameter)
	{
		var ((logger, workspaces), stop) = parameter;
		await using var workspace = workspaces.Get();
		if (await _first.Off(new(workspace.Destination, stop)))
		{
			var to      = workspace.Destination.Set<T>();
			var cleared = await to.ExecuteDeleteAsync(stop).Off();
			logger.LogInformation("Flatten {Set}: Cleared of {Count} entries", to.GetType(), cleared);
		}
	}

	public ValueTask Get(Stop<EntityPostMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityMigratorInput> parameter) => _previous.Get(parameter);
}