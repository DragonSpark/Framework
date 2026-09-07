using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;
using DragonSpark.Application.AspNet.Entities.Migration.Steps;
using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public static class Extensions
{
	extension(IMigrationSteps @this)
	{
		public IMigrationSteps WithUpdateAwareness() => new UpdateAwareMigrationSteps(@this);

		public IMigrationSteps WithConstraintManagement(DbContext destination)
			=> new ConstraintAwareMigrationSteps(@this, destination.Database);

		public IMigrationSteps WithName(string name)
			=> new NameAwareMigrationSteps(@this, name);
	}

	extension(IEntityMigratorSelector @this)
	{
		public IEntityMigratorSelector Exact(params Type[] matches)
			=> new ExactAwareEntityMigratorSelector(@this, matches);

		public IEntityMigratorSelector Ignoring(params Type[] matches)
			=> new IgnoreAwareEntityMigratorSelector(@this, matches);

		public IEntityMigratorSelector WithIdentityAwareness() => new IdentityAwareEntityMigratorSelector(@this);

		public IEntityMigratorSelector WithExceptionAwareness() => new ExceptionAwareEntityMigratorSelector(@this);
	}

	extension(IInfrastructure<IServiceProvider> @this)
	{
		public DbContext Context() => @this.Instance.GetRequiredService<DbContext>();
	}

	extension(EntityEntry @this)
	{
		public EntityEntry<T> Of<T>() where T : class => @this.To<EntityEntry<T>>();

		public Task Load(CancellationToken stop)
			=> @this.State == EntityState.Detached ? @this.ReloadAsync(stop) : Task.CompletedTask;
	}

	extension<TEntity>(EntityEntry<TEntity> entry) where TEntity : class
	{
		public Task Include<TProperty>(Expression<Func<TEntity, TProperty>> path, CancellationToken token = default)
			=> LoadMembers.Default.Allocate(new(new(path.Body, entry), token));
	}
}