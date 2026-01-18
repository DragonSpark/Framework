using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Application.Diagnostics.Initialization;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public class Migration<T> : Migration
{
	protected Migration(DbContext source, DbContext destination, IEntityMigrators processors)
		: this(processors.Get(new(source, destination))) {}

	protected Migration(params IEntityMigrator[] migrators) : this(DefaultLog<T>.Default.Get(), migrators) {}

	protected Migration(ILogger logger, params IEntityMigrator[] migrators) : base(logger, migrators) {}
}

public class Migration : IMigration, ICommand
{
	readonly ILogger                _logger;
	readonly Array<IEntityMigrator> _migrators;

	protected Migration(ILogger logger, params IEntityMigrator[] migrators)
	{
		_logger    = logger;
		_migrators = migrators;
	}

	public void Execute(None parameter)
	{
		Execute(DefaultBatchSize.Default);
	}

	public void Execute(ushort parameter)
	{
		var input = new EntityMigratorInput(_logger, parameter);
		foreach (var batch in _migrators)
		{
			batch.Execute(input);
		}
	}
}