using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IEntityMigrators : IArray<MigrationInput, IEntityMigrator>;