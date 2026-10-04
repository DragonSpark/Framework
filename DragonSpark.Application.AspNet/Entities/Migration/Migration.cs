using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Application.AspNet.Entities.Migration.Steps;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public class Migration : IMigration
{
	readonly ILogger               _logger;
	readonly IWorkspaceDefinition  _workspaces;
	readonly Array<IMigrationStep> _steps;

	// ReSharper disable once TooManyDependencies
	protected Migration(ILogger logger, IWorkspaceDefinition definition, IEntityMigrators processors,
	                    IMigrationSteps steps)
		: this(logger, definition, steps, processors.Get(definition)) {}

	// ReSharper disable once TooManyDependencies
	protected Migration(ILogger logger, IWorkspaceDefinition definition, IMigrationSteps steps,
	                    params IEntityMigrator[] migrators)
		: this(logger, definition, [.. steps.Get(migrators)]) {}

	protected Migration(ILogger logger, IWorkspaceDefinition workspaces, params IMigrationStep[] steps)
	{
		_logger     = logger;
		_workspaces = workspaces;
		_steps      = steps;
	}

	public async ValueTask Get(Stop<ushort> parameter)
	{
		var (subject, stop) = parameter;
		var updated = new EntityMigratorInput(_logger, _workspaces, subject);
		var input   = updated.Stop(stop);
		foreach (var step in _steps.Open())
		{
			await step.Off(input);
		}
	}
}