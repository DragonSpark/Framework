using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct EntityMigratorInput(ILogger Logger, IDestination Destination, ushort BatchSize)
{
	public EntityMigratorInput(ILogger logger, IDestination destination)
		: this(logger, destination, DefaultBatchSize.Default) {}
}