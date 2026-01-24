using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using Microsoft.EntityFrameworkCore;
using System;

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
}