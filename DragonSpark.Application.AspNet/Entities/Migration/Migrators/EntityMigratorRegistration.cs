using DragonSpark.Model.Results;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public sealed class EntityMigratorRegistration : Instance<KeyValuePair<Type, IEntityMigrator>>
{
	public EntityMigratorRegistration(IEntityMigrator instance) : this(instance.Get().From, instance) {}

	public EntityMigratorRegistration(Type @for, IEntityMigrator instance) : base(new(@for, instance)) {}
}