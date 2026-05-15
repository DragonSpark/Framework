namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public static class Extensions
{
	public static EntityMigratorRegistration Registered(this IEntityMigrator @this) => new(@this);
}