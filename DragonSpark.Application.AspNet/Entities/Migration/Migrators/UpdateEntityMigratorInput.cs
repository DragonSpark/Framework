using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct UpdateEntityMigratorInput(ILogger Logger, ushort BatchSize);