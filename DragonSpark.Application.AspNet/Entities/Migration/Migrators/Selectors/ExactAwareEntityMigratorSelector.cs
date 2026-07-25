using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using DragonSpark.Model.Selection;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;

sealed class ExactAwareEntityMigratorSelector : IEntityMigratorSelector
{
	readonly IEntityMigratorSelector                                _previous;
	readonly ImmutableHashSet<Type>                                 _matches;
	readonly ISelect<ConstructEntityMigratorInput, IEntityMigrator> _exact;

	public ExactAwareEntityMigratorSelector(IEntityMigratorSelector previous, params Type[] matches)
		: this(previous, matches.ToImmutableHashSet(), ConstructExactEntityMigrator.Default) {}

	public ExactAwareEntityMigratorSelector(IEntityMigratorSelector previous, ImmutableHashSet<Type> matches,
	                                        ISelect<ConstructEntityMigratorInput, IEntityMigrator> exact)
	{
		_previous = previous;
		_matches  = matches;
		_exact    = exact;
	}

	public IEntityMigrator? Get(EntityMigratorSelectorInput parameter)
		=> _matches.Contains(parameter.Result.From.ClrType)
		   && parameter.Result is MatchedEntityComparisonResult(var from, var to)
			   ? _exact.Get(new(parameter.Source, parameter.Destination, from, to))
			   : _previous.Get(parameter);
}