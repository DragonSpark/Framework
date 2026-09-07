using DragonSpark.Application.AspNet.Entities.Migration.Identity;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;

sealed class IdentityAwareEntityMigratorSelector : IEntityMigratorSelector
{
	readonly IEntityMigratorSelector _previous;
	readonly ICondition<IEntityType> _identity;

	public IdentityAwareEntityMigratorSelector(IEntityMigratorSelector previous)
		: this(previous, IsIdentityEntity.Default) {}

	public IdentityAwareEntityMigratorSelector(IEntityMigratorSelector previous, ICondition<IEntityType> identity)
	{
		_previous = previous;
		_identity = identity;
	}

	public IEntityMigrator? Get(EntityMigratorSelectorInput parameter)
	{
		var previous = _previous.Get(parameter);
		if (previous?.Get() is var (_, to))
		{
			var (_, destination, _) = parameter;
			var entityType = destination.Model.FindEntityType(to);
			if (entityType is not null && _identity.Get(entityType))
			{
				var migrator = new IdentityAwareEntityMigrator(previous, destination.Database, entityType);
				return previous is IUpdateAwareEntityMigrator
					       ? new UpdateAwareEntityMigrator(migrator, previous)
					       : migrator;
			}
		}

		return previous;
	}
}