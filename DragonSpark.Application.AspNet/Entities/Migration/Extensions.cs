using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public static class Extensions
{
	public static IMigrationSteps WithUpdateAwareness(this IMigrationSteps @this)
		=> new UpdateAwareMigrationSteps(@this);

	public static IMigrationSteps WithConstraintManagement(this IMigrationSteps @this, DbContext destination)
		=> new ConstraintAwareMigrationSteps(@this, destination.Database);

	public static IEntityMigratorSelector Exact(this IEntityMigratorSelector @this, params Type[] matches)
		=> new ExactAwareEntityMigratorSelector(@this, matches);

	public static IEntityMigratorSelector Ignoring(this IEntityMigratorSelector @this, params Type[] matches)
		=> new IgnoreAwareEntityMigratorSelector(@this, matches);

	public static IEntityMigratorSelector WithIdentityAwareness(this IEntityMigratorSelector @this)
		=> new IdentityAwareEntityMigratorSelector(@this);

	public static IEntityMigratorSelector WithExceptionAwareness(this IEntityMigratorSelector @this)
		=> new ExceptionAwareEntityMigratorSelector(@this);

	public static DbContext Context(this IInfrastructure<IServiceProvider> @this)
		=> @this.Instance.GetRequiredService<DbContext>();

	public static EntityEntry<T> Of<T>(this EntityEntry @this) where T : class => @this.To<EntityEntry<T>>();

	public static Task Include<TEntity, TProperty>(this EntityEntry<TEntity> entry,
	                                               Expression<Func<TEntity, TProperty>> path,
	                                               CancellationToken token = default)
		where TEntity : class
		=> LoadMembers.Default.Allocate(new(new(path.Body, entry), token));
}