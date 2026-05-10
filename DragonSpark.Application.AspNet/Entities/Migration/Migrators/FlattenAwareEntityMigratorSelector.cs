using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Types;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public class FlattenAwareEntityMigratorSelector : IEntityMigratorSelector
{
	readonly IEntityMigratorSelector                               _previous;
	readonly IGeneric<IEntityMigrator, DbContext, IEntityMigrator> _generic;
	readonly Array<Type>                                           _candidates;

	protected FlattenAwareEntityMigratorSelector(params Type[] candidates)
		: this(EntityMigratorSelector.Default,
		       Start.A.Generic(typeof(FlattenAwareEntityMigrator<>))
		            .Of.Type<IEntityMigrator>()
		            .WithParameterOf<IEntityMigrator>()
		            .AndOf<DbContext>(),
		       candidates) {}

	public FlattenAwareEntityMigratorSelector(IEntityMigratorSelector previous,
	                                          IGeneric<IEntityMigrator, DbContext, IEntityMigrator> generic,
	                                          params Type[] candidates)
	{
		_previous   = previous;
		_generic    = generic;
		_candidates = candidates;
	}

	public IEntityMigrator? Get(EntityMigratorSelectorInput parameter)
	{
		var (_, destination, r) = parameter;
		var previous = _previous.Get(parameter);
		var result = previous is not null && r is MatchedEntityComparisonResult(var from, var to)
		                                  && _candidates.Open().Contains(from.ClrType)
			             ? _generic.Get(to.ClrType)(previous, destination)
			             : previous;
		return result;
	}
}