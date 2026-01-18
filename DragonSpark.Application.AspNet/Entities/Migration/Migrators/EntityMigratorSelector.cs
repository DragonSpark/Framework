using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using DragonSpark.Compose;
using DragonSpark.Reflection.Types;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class EntityMigratorSelector : IEntityMigratorSelector
{
	public static EntityMigratorSelector Default { get; } = new();

	EntityMigratorSelector()
		: this(Start.A.Generic(typeof(EntityMigrator<,>))
		            .Of.Type<IEntityMigrator>()
		            .WithParameterOf<DbContext>()
		            .AndOf<DbContext>()) {}

	readonly IGeneric<DbContext, DbContext, IEntityMigrator> _generic;

	public EntityMigratorSelector(IGeneric<DbContext, DbContext, IEntityMigrator> generic) => _generic = generic;

	public IEntityMigrator? Get(EntityMigratorSelectorInput parameter)
	{
		var (source, destination, result) = parameter;
		return result switch
		{
			ExactEntityComparisonResult(var from, var to)
				=> _generic.Get(from.ClrType, to.ClrType)(source, destination),
			MissingEntityComparisonResult => null,
			_ => throw new InvalidOperationException($"Could not find entity migrator for {result.From}")
		};
	}
}