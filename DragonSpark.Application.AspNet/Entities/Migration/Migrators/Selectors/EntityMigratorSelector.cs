using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using DragonSpark.Model.Selection;
using DragonSpark.Text;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;

sealed class EntityMigratorSelector : IEntityMigratorSelector
{
	public static EntityMigratorSelector Default { get; } = new();

	EntityMigratorSelector()
		: this(ConstructExactEntityMigrator.Default, ModifiedEntityComparisonResultFormatter.Default) {}

	readonly ISelect<ConstructEntityMigratorInput, IEntityMigrator> _exact;
	readonly IFormatter<ModifiedEntityComparisonResult>           _formatter;

	public EntityMigratorSelector(ISelect<ConstructEntityMigratorInput, IEntityMigrator> exact,
	                              IFormatter<ModifiedEntityComparisonResult> formatter)
	{
		_exact     = exact;
		_formatter = formatter;
	}

	public IEntityMigrator? Get(EntityMigratorSelectorInput parameter)
	{
		var (source, destination, result) = parameter;
		return result switch
		{
			ExactEntityComparisonResult(var from, var to) => _exact.Get(new(source, destination, from, to)),
			MissingEntityComparisonResult => null,
			ModifiedEntityComparisonResult modified => throw new InvalidOperationException(_formatter.Get(modified)),
			_ => throw new InvalidOperationException($"Could not find entity migrator for {result.From}")
		};
	}
}