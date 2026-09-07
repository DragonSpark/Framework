using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;

public class FlattenAwareEntityMigratorSelector : IEntityMigratorSelector
{
	readonly IEntityMigratorSelector                    _previous;
	readonly IGeneric<IEntityMigrator, IEntityMigrator> _generic;
	readonly Array<Type>                                _candidates;

	protected FlattenAwareEntityMigratorSelector(params Type[] candidates)
		: this(EntityMigratorSelector.Default,
		       Start.A.Generic(typeof(FlattenAwareEntityMigrator<>))
		            .Of.Type<IEntityMigrator>()
		            .WithParameterOf<IEntityMigrator>(),
		       candidates) {}

	public FlattenAwareEntityMigratorSelector(IEntityMigratorSelector previous,
	                                          IGeneric<IEntityMigrator, IEntityMigrator> generic,
	                                          params Type[] candidates)
	{
		_previous   = previous;
		_generic    = generic;
		_candidates = candidates;
	}

	public IEntityMigrator? Get(EntityMigratorSelectorInput parameter)
	{
		var (_, _, r) = parameter;
		var previous = _previous.Get(parameter);
		var result = previous is not null && r is MatchedEntityComparisonResult(var from, var to)
		                                  && _candidates.Open().Contains(from.ClrType)
			             ? _generic.Get(to.ClrType)(previous)
			             : previous;
		return result;
	}
}