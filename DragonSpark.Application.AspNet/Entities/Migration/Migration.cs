using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Application.AspNet.Entities.Migration.Steps;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public class Migration : IMigration
{
	readonly ILogger               _logger;
	readonly ushort                _batchSize;
	readonly Array<IMigrationStep> _steps;

	// ReSharper disable once TooManyDependencies
	protected Migration(ILogger logger, MigrationInput input, IEntityMigrators processors, IMigrationSteps steps)
		: this(logger, steps, processors.Get(input)) {}

	protected Migration(ILogger logger, IMigrationSteps steps, params IEntityMigrator[] migrators)
		: this(logger, [.. steps.Get(migrators)]) {}

	protected Migration(ILogger logger, params IMigrationStep[] steps)
		: this(logger, DefaultBatchSize.Default, steps) {}

	protected Migration(ILogger logger, ushort batchSize, params IMigrationStep[] steps)
	{
		_logger    = logger;
		_batchSize = batchSize;
		_steps     = steps;
	}

	public async ValueTask Get(Stop<ushort> parameter)
	{
		var input = new EntityMigratorInput(_logger, parameter).Stop(parameter);
		foreach (var step in _steps.Open())
		{
			await step.Off(input);
		}
	}

	public ValueTask Get(CancellationToken parameter) => Get(new(_batchSize, parameter));
}