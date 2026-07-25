using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class CompositeEntityMigrators : Instance<EntityTypeMapping>, IExtendedEntityMigrator
{
	readonly Array<IEntityMigrator> _migrators;

	public CompositeEntityMigrators(ReadOnlyMemory<IEntityMigrator> migrators)
		: this(migrators.AsValueEnumerable().Select(x => x.Get()).Distinct().Single().Verified(),
		       migrators.ToArray()) {}

	public CompositeEntityMigrators(EntityTypeMapping mapping, params IEntityMigrator[] migrators) : base(mapping)
		=> _migrators = migrators;

	public async ValueTask Get(Stop<EntityPreMigrationInput> parameter)
	{
		foreach (var migrator in _migrators.Open())
		{
			await migrator.Off(parameter);
		}
	}

	public async ValueTask Get(Stop<EntityPostMigrationInput> parameter)
	{
		foreach (var migrator in _migrators.Open())
		{
			await migrator.Off(parameter);
		}
	}

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		foreach (var migrator in _migrators.Open())
		{
			await migrator.Off(parameter);
		}
	}

	public async ValueTask Get(Stop<UpdateEntityMigratorInput> parameter)
	{
		foreach (var migrator in _migrators.Open().OfType<IUpdateAwareEntityMigrator>())
		{
			await migrator.Off(parameter);
		}
	}
}