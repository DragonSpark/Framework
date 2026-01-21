namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class UpdateAwareEntityMigrator : IExtendedEntityMigrator
{
	readonly IEntityMigrator _previous, _update;

	public UpdateAwareEntityMigrator(IEntityMigrator previous, IEntityMigrator update)
	{
		_previous = previous;
		_update   = update;
	}

	public void Execute(EntityPreMigrationInput parameter)
	{
		_previous.Execute(parameter);
	}

	public void Execute(EntityPostMigrationInput parameter)
	{
		_previous.Execute(parameter);
	}

	public void Execute(EntityMigratorInput parameter)
	{
		_previous.Execute(parameter);
	}

	public EntityTypeMapping Get() => _previous.Get();

	public void Execute(UpdateEntityMigratorInput parameter)
	{
		var (logger, batchSize) = parameter;
		_update.Execute(new EntityMigratorInput(logger, batchSize));
	}
}