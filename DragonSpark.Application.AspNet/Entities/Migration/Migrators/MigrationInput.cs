using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public sealed record MigrationInput(DbContext Source, DbContext Destination);