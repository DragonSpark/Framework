using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public abstract class ExtendedEntityMigratorBase<TFrom, TTo> : IExtendedEntityMigrator where TFrom : class where TTo : class
{
	readonly IExtendedEntityMigrator _migrator;

	protected ExtendedEntityMigratorBase(DbContext source, DbContext destination, IMap secondary)
		: this(new(source, destination), Map.Default, secondary) {}

	protected ExtendedEntityMigratorBase(Contexts pair, IMap primary, IMap secondary)
		: this(new EntityMigrator<TFrom, TTo>(pair.Source, pair.Destination, primary), 
		       new EntityMigrator<TFrom, TTo>(pair.Source, pair.Destination, secondary)) {}

	protected ExtendedEntityMigratorBase(IEntityMigrator previous, IEntityMigrator update)
		: this(new UpdateAwareEntityMigrator(previous, update)) {}

	protected ExtendedEntityMigratorBase(IExtendedEntityMigrator migrator) => _migrator = migrator;

	public void Execute(EntityPreMigrationInput parameter)
	{
		_migrator.Execute(parameter);
	}

	public void Execute(EntityPostMigrationInput parameter)
	{
		_migrator.Execute(parameter);
	}

	public void Execute(EntityMigratorInput parameter)
	{
		_migrator.Execute(parameter);
	}

	public EntityTypeMapping Get() => _migrator.Get();

	public void Execute(UpdateEntityMigratorInput parameter)
	{
		_migrator.Execute(parameter);
	}
}