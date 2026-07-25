using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;

sealed class IgnoreAwareEntityMigratorSelector : IEntityMigratorSelector
{
	readonly IEntityMigratorSelector _previous;
	readonly ImmutableHashSet<Type>  _matches;

	public IgnoreAwareEntityMigratorSelector(IEntityMigratorSelector previous, params Type[] matches)
		: this(previous, matches.ToImmutableHashSet()) {}

	public IgnoreAwareEntityMigratorSelector(IEntityMigratorSelector previous, ImmutableHashSet<Type> matches)
	{
		_previous = previous;
		_matches  = matches;
	}

	public IEntityMigrator? Get(EntityMigratorSelectorInput parameter)
		=> _matches.Contains(parameter.Result.From.ClrType)
			   ? null
			   : _previous.Get(parameter);
}