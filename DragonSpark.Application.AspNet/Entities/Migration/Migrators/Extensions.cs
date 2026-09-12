using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public static class Extensions
{
	extension(IEntityMigrator @this)
	{
		public EntityMigratorRegistration Registered() => new(@this);

		public IEntityMigrator AsPost(IWorkspaceDefinition definition)
			=> new PostEntityMigrator(@this, definition);
	}

	public static IEntityMigrators Configured(this IEntityMigrators @this, Action<IWorkspaceDefinition> configure)
		=> new ConfiguredEntityMigrators(@this, configure);

	public static IQueryable<T> ExactSet<T>(this DbContext @this) where T : class => @this.Set<T>().Exact();

	public static IQueryable<T> Exact<T>(this DbSet<T> @this) where T : class
		=> Migrators.ExactSet<T>.Default.Get(@this);

	
}