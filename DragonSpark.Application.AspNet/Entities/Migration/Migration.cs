using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Application.Diagnostics.Initialization;
using DragonSpark.Model;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public class Migration<T> : Migration
{
	protected Migration(MigrationInput input, IEntityMigrators processors, IMigrationSteps steps)
		: this(steps, processors.Get(input)) {}

	protected Migration(IMigrationSteps steps, params IEntityMigrator[] migrators)
		: this(DefaultLog<T>.Default.Get(), steps.Get(migrators).ToArray()) {}

	protected Migration(ILogger logger, params IMigrationStep[] steps) : base(logger, steps) {}
}

public class Migration : IMigration
{
	readonly ILogger          _logger;
	readonly IMigrationStep[] _steps;

	protected Migration(ILogger logger, params IMigrationStep[] steps)
	{
		_logger = logger;
		_steps  = steps;
	}

	public void Execute(None parameter)
	{
		Execute(DefaultBatchSize.Default);
	}

	public void Execute(ushort parameter)
	{
		var input = new EntityMigratorInput(_logger, parameter);
		foreach (var step in _steps)
		{
			step.Execute(input);
		}
	}
}