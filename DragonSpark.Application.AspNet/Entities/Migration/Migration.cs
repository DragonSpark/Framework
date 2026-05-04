using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Application.Diagnostics.Initialization;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
	readonly ILogger               _logger;
	readonly ushort                _batchSize;
	readonly Array<IMigrationStep> _steps;

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