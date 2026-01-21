using DragonSpark.Model.Results;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public sealed class EntityMigratorRegistration : Instance<KeyValuePair<Type, IEntityMigrator>>
{
	public EntityMigratorRegistration(IEntityMigrator instance) : base(new(instance.Get().From, instance)) {}
}

// TODO

public static class Extensions
{
	public static EntityMigratorRegistration Registered(this IEntityMigrator @this) => new(@this);

	
}