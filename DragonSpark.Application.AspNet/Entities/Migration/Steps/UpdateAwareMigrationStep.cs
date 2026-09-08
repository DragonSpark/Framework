using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

public sealed class UpdateAwareMigrationStep : IMigrationBody
{
	readonly IMigrationStep                    _previous;
	readonly Array<IUpdateAwareEntityMigrator> _migrators;

	public UpdateAwareMigrationStep(IMigrationStep previous, Array<IUpdateAwareEntityMigrator> migrators)
	{
		_previous  = previous;
		_migrators = migrators;
	}

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var ((logger, workspaces, batchSize), stop) = parameter;
		await _previous.Off(parameter);
		var input = new UpdateEntityMigratorInput(logger, workspaces, batchSize).Stop(stop);
		foreach (var migrator in _migrators.Open())
		{
			await migrator.Off(input);
		}
	}
}

// TODO

sealed class UpdateAwareWorkspaces : IWorkspaces
{
	readonly IWorkspaces _previous;

	public UpdateAwareWorkspaces(IWorkspaces previous) => _previous = previous;

	public Workspace Get() => _previous.Get();

	public DatabaseFacade Database => _previous.Database;

	public IModel Model => _previous.Model;
}