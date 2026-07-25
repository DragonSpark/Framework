using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class UpdateAwareEntityMigrator : IExtendedEntityMigrator
{
	readonly IEntityMigrator _previous, _update;

	public UpdateAwareEntityMigrator(IEntityMigrator previous, IEntityMigrator update)
	{
		_previous = previous;
		_update   = update;
	}

	public EntityTypeMapping Get() => _previous.Get();

	public ValueTask Get(Stop<EntityPreMigrationInput> parameter) => _previous.Get(parameter);

	public ValueTask Get(Stop<EntityPostMigrationInput> parameter) => _previous.Get(parameter);

	public ValueTask Get(Stop<EntityMigratorInput> parameter) => _previous.Get(parameter);

	public ValueTask Get(Stop<UpdateEntityMigratorInput> parameter)
	{
		var ((logger, batchSize), stop) = parameter;
		return _update is IExtendedEntityMigrator extended
			       ? extended.Get(parameter)
			       : _update.Get(new EntityMigratorInput(logger, batchSize).Stop(stop));
	}
}