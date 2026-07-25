namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public static class Extensions
{
	public static EntityMigratorRegistration Registered(this IEntityMigrator @this) => new(@this);

	public static IEntityMigrators Configured(this IEntityMigrators @this, Action<MigrationInput> configure)
		=> new ConfiguredEntityMigrators(@this, configure);
}