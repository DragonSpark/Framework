using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public class CompositeEntityMigrator : Instance<EntityTypeMapping>, IExtendedEntityMigrator
{
	readonly Array<IEntityMigrator> _migrators;

	public CompositeEntityMigrator(params IEntityMigrator[] migrators) : this(migrators.AsMemory()) {}

	public CompositeEntityMigrator(ReadOnlyMemory<IEntityMigrator> migrators)
		: this(migrators.AsValueEnumerable().Select(x => x.Get()).Distinct().Single().Verified(),
		       migrators.ToArray()) {}

	public CompositeEntityMigrator(EntityTypeMapping mapping, params IEntityMigrator[] migrators) : base(mapping)
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