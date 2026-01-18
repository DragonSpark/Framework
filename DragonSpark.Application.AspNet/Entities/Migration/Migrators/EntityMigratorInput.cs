using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct EntityMigratorInput(ILogger Logger, ushort BatchSize)
{
	public EntityMigratorInput(ILogger logger) : this(logger, DefaultBatchSize.Default) {}
}