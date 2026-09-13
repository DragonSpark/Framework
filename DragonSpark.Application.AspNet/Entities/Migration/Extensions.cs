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

		public IMigrationSteps WithName(string name) => new NameAwareMigrationSteps(@this, name);
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

		public Task<T> Load<T>(Func<IQueryable<T>, IQueryable<T>> include, CancellationToken token)
			where T : class
			=> AspNet.Entities.Migration.Load<T>.Default.Get(new(new(@this, include), token));

		public Task Load(CancellationToken stop)
			=> @this.State == EntityState.Detached ? @this.ReloadAsync(stop) : Task.CompletedTask;
	}

	extension<T>(EntityEntry<T> @this) where T : class
	{
		public Task<T> Load(Func<IQueryable<T>, IQueryable<T>> include, CancellationToken token)
			=> AspNet.Entities.Migration.Load<T>.Default.Get(new(new(@this, include), token));

		public Task Include<TProperty>(Expression<Func<T, TProperty>> path, CancellationToken token)
			=> LoadMembers.Default.Allocate(new(new(path.Body, @this), token));

		public EntityEntry<T> Assigned(EntityEntry source) => @this.Assigned(source.CurrentValues);

		public EntityEntry<T> Assigned(PropertyValues source)
		{
			AspNet.Entities.Migration.Identified.Default.Execute(new(source, @this.CurrentValues));
			return @this;
		}
	}

	extension(EntityEntry @this)
	{
		public EntityEntry Identified(EntityEntry source) => @this.Identified(source.CurrentValues);

		public EntityEntry Identified(PropertyValues source)
		{
			AspNet.Entities.Migration.Identified.Default.Execute(new(source, @this.CurrentValues));
			return @this;
		}
	}

	extension(DbContext @this)
	{
		public EntityEntry Applied(EntityEntry entry)
			=> AspNet.Entities.Migration.Applied.Default.Get(new(@this, entry));
	}
}