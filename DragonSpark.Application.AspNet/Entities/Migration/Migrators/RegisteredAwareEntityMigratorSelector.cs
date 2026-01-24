using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
		: this(registrations.ToLookup(x => x.Key)
		                    .ToDictionary(x => x.Key, x => ComposeEntityMigrators.Default.Get(x.Select(y => y.Value)))
		                    .ToStore(), previous) {}

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

// TODO

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

public class EntityMigratorSelectorInstance : Instance<IEntityMigratorSelector>
{
	protected EntityMigratorSelectorInstance(IEntityMigratorSelector start, Array<Type> ignore, Array<Type> exact)
		: base(start.Ignoring(ignore).Exact(exact).WithIdentityAwareness().WithExceptionAwareness()) {}
}