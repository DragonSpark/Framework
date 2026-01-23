using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class CompositeEntityMigrators : Instance<EntityTypeMapping>, IEntityMigrator
{
	readonly Array<IEntityMigrator> _migrators;

	public CompositeEntityMigrators(ReadOnlyMemory<IEntityMigrator> migrators)
		: this(migrators.AsValueEnumerable().Select(x => x.Get()).Distinct().Single().Verified(),
		       migrators.ToArray()) {}

	public CompositeEntityMigrators(EntityTypeMapping mapping, params IEntityMigrator[] migrators) : base(mapping)
		=> _migrators = migrators;

	public void Execute(EntityPreMigrationInput parameter)
	{
		foreach (var migrator in _migrators)
		{
			migrator.Execute(parameter);
		}
	}

	public void Execute(EntityPostMigrationInput parameter)
	{
		foreach (var migrator in _migrators)
		{
			migrator.Execute(parameter);
		}
	}

	public void Execute(EntityMigratorInput parameter)
	{
		foreach (var migrator in _migrators)
		{
			migrator.Execute(parameter);
		}
	}
}