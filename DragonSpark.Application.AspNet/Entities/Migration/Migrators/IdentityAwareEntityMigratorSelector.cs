using DragonSpark.Application.AspNet.Entities.Migration.Identity;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

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
		if (previous is not null)
		{
			var (_, to) = previous.Get();
			var entityType = parameter.Destination.Model.FindEntityType(to);
			if (entityType is not null && _identity.Get(entityType))
			{
				var stop = entityType.Name == "Starbeam.Entities.Identity.Settings.UserSettings";
				return new IdentityAwareEntityMigrator(previous, parameter.Destination, entityType);
			}
		}

		return previous;
	}
}