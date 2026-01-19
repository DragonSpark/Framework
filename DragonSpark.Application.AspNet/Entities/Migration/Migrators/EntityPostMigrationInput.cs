using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct EntityPostMigrationInput(ILogger Logger);