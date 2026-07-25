using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public abstract class ExtendedEntityMigratorBase<TFrom, TTo> : IExtendedEntityMigrator where TFrom : class where TTo : class
{
	readonly IExtendedEntityMigrator _migrator;

	protected ExtendedEntityMigratorBase(DbContext source, DbContext destination, 
	                                     Func<Stop<MapInput<TFrom, TTo>>, ValueTask> map)
		: this(source, destination, new Map<TFrom,TTo>(map, EmptyMap.Default)) {}
	protected ExtendedEntityMigratorBase(DbContext source, DbContext destination, Action<MapInput<TFrom, TTo>> map)
		: this(source, destination, new Map<TFrom,TTo>(map, EmptyMap.Default)) {}

	protected ExtendedEntityMigratorBase(DbContext source, DbContext destination, IMap secondary)
		: this(new Contexts<TFrom>(source, destination), Map.Default, secondary) {}

	protected ExtendedEntityMigratorBase(Contexts<TFrom> contexts, IMap secondary)
		: this(contexts, Map.Default, secondary) {}

	protected ExtendedEntityMigratorBase(Contexts<TFrom> contexts, IMap primary, IMap secondary)
		: this(new EntityMigrator<TFrom, TTo>(contexts, primary), 
		       new EntityMigrator<TFrom, TTo>(contexts, new UpdateEntityProcessor<TFrom,TTo>(secondary))) {}

	protected ExtendedEntityMigratorBase(IEntityMigrator previous, IEntityMigrator update)
		: this(new UpdateAwareEntityMigrator(previous, update)) {}

	protected ExtendedEntityMigratorBase(IExtendedEntityMigrator migrator) => _migrator = migrator;

	public EntityTypeMapping Get() => _migrator.Get();

	public ValueTask Get(Stop<EntityPreMigrationInput> parameter) => _migrator.Get(parameter);

	public ValueTask Get(Stop<EntityPostMigrationInput> parameter) => _migrator.Get(parameter);

	public ValueTask Get(Stop<EntityMigratorInput> parameter) => _migrator.Get(parameter);

	public ValueTask Get(Stop<UpdateEntityMigratorInput> parameter) => _migrator.Get(parameter);
}