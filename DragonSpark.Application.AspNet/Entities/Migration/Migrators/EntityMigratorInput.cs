using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct EntityMigratorInput(ILogger Logger, IContexts Destination, ushort BatchSize)
{
	public EntityMigratorInput(ILogger logger, IContexts destination)
		: this(logger, destination, DefaultBatchSize.Default) {}
}