using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public static class Extensions
{
	public static EntityMigratorRegistration Registered(this IEntityMigrator @this) => new(@this);

	public static IEntityMigrators Configured(this IEntityMigrators @this, Action<MigrationInput> configure)
		=> new ConfiguredEntityMigrators(@this, configure);

	public static IQueryable<T> ExactSet<T>(this DbContext @this) where T : class => @this.Set<T>().Exact();

	public static IQueryable<T> Exact<T>(this DbSet<T> @this) where T : class
		=> Migrators.ExactSet<T>.Default.Get(@this);
}