using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using DragonSpark.Compose;
using DragonSpark.Reflection.Types;
using DragonSpark.Text;
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
		            .AndOf<DbContext>(),
		       ModifiedEntityComparisonResultFormatter.Default) {}

	readonly IGeneric<DbContext, DbContext, IEntityMigrator> _generic;
	readonly IFormatter<ModifiedEntityComparisonResult>      _formatter;

	public EntityMigratorSelector(IGeneric<DbContext, DbContext, IEntityMigrator> generic,
	                              IFormatter<ModifiedEntityComparisonResult> formatter)
	{
		_generic   = generic;
		_formatter = formatter;
	}

	public IEntityMigrator? Get(EntityMigratorSelectorInput parameter)
	{
		var (source, destination, result) = parameter;
		return result switch
		{
			ExactEntityComparisonResult(var from, var to) =>
				_generic.Get(from.ClrType, to.ClrType)(source, destination),
			MissingEntityComparisonResult => null,
			ModifiedEntityComparisonResult modified => throw new InvalidOperationException(_formatter.Get(modified)),
			_ => throw new InvalidOperationException($"Could not find entity migrator for {result.From}")
		};
	}
}