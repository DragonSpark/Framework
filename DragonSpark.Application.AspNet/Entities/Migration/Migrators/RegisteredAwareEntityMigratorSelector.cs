using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public class RegisteredAwareEntityMigratorSelector : IEntityMigratorSelector
{
	readonly IConditional<Type, IEntityMigrator> _registered;
	readonly IEntityMigratorSelector             _previous;

	protected RegisteredAwareEntityMigratorSelector(params KeyValuePair<Type, IEntityMigrator>[] registrations)
		: this(EntityMigratorSelector.Default, registrations) {}

	protected RegisteredAwareEntityMigratorSelector(IEntityMigratorSelector previous,
	                                                params KeyValuePair<Type, IEntityMigrator>[] registrations)
		: this(registrations.ToDictionary().ToStore(), previous) {}

	protected RegisteredAwareEntityMigratorSelector(IConditional<Type, IEntityMigrator> registered,
	                                                IEntityMigratorSelector previous)
	{
		_registered = registered;
		_previous   = previous;
	}

	public IEntityMigrator? Get(EntityMigratorSelectorInput parameter)
		=> _registered.TryGet(parameter.Result.From.ClrType, out var registered)
			   ? registered
			   : _previous.Get(parameter);
}