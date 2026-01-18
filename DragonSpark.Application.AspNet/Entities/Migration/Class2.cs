using DragonSpark.Application.AspNet.Entities.Migration.Planning;
using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace DragonSpark.Application.AspNet.Entities.Migration;

class Class2;

public sealed record MigrationInput(DbContext Source, DbContext Destination);

public interface IEntityMigrators : IArray<MigrationInput, IEntityMigrator>;

public class EntityMigrators : IEntityMigrators
{
	readonly IArray<IModel, IEntityType>     _order;
	readonly IComposeEntityComparisonResults _results;
	readonly IEntityMigratorSelector         _selector;

	protected EntityMigrators(IModelTypes types, IEntityMigratorSelector selector)
		: this(MigrationOrder.Default, new ComposeEntityComparisonResults(types), selector) {}

	protected EntityMigrators(IArray<IModel, IEntityType> order, IComposeEntityComparisonResults results,
	                          IEntityMigratorSelector selector)
	{
		_order    = order;
		_results  = results;
		_selector = selector;
	}

	public Array<IEntityMigrator> Get(MigrationInput parameter)
	{
		var (source, destination) = parameter;
		var       order   = _order.Get(source.Model);
		using var results = _results.Get(new(order, destination.Model));
		using var result  = ArrayBuilder.New<IEntityMigrator>(results.Length);
		foreach (var item in results)
		{
			var batch = _selector.Get(new(source, destination, item));
			if (batch is not null)
			{
				result.UncheckedAdd(batch);
			}
		}

		return result;
	}
}

sealed class EntityMigrator<TFrom, TTo> : EntityMigratorBase<TFrom, TTo> where TFrom : class where TTo : class
{
	public EntityMigrator(DbContext source, DbContext destination) : this(source, destination, Map.Default) {}

	public EntityMigrator(DbContext source, DbContext destination, IMap map) : this(new(source, destination), map) {}

	public EntityMigrator(Batching<TFrom> batching, IMap map) : base(batching, map) {}
}

public readonly record struct EntityMigratorSelectorInput(
	DbContext Source,
	DbContext Destination,
	EntityComparisonResult Result);

public interface IEntityMigratorSelector : ISelect<EntityMigratorSelectorInput, IEntityMigrator?> {}

public class FlattenAwareEntityMigratorSelector : IEntityMigratorSelector
{
	readonly IEntityMigratorSelector                                          _previous;
	readonly IGeneric<IEntityMigrator, DbContext, DbContext, IEntityMigrator> _generic;
	readonly Array<Type>                                                      _candidates;

	protected FlattenAwareEntityMigratorSelector(params Type[] candidates)
		: this(EntityMigratorSelector.Default,
		       Start.A.Generic(typeof(FlattenAwareEntityMigrator<,>))
		            .Of.Type<IEntityMigrator>()
		            .WithParameterOf<IEntityMigrator>()
		            .AndOf<DbContext>()
		            .AndOf<DbContext>(),
		       candidates) {}

	public FlattenAwareEntityMigratorSelector(IEntityMigratorSelector previous,
	                                          IGeneric<IEntityMigrator, DbContext, DbContext, IEntityMigrator> generic,
	                                          params Type[] candidates)
	{
		_previous   = previous;
		_generic    = generic;
		_candidates = candidates;
	}

	public IEntityMigrator? Get(EntityMigratorSelectorInput parameter)
	{
		var (source, destination, r) = parameter;
		var previous = _previous.Get(parameter);
		var result = previous is not null && r is MatchedEntityComparisonResult(var from, var to)
		                                  && _candidates.Open().Contains(from.ClrType)
			             ? _generic.Get(from.ClrType, to.ClrType)(previous, source, destination)
			             : previous;
		return result;
	}
}

sealed class FlattenAwareEntityMigrator<TFrom, TTo> : IEntityMigrator where TFrom : class where TTo : class
{
	readonly IEntityMigrator _previous;
	readonly DbContext       _source, _destination;

	public FlattenAwareEntityMigrator(IEntityMigrator previous, DbContext source, DbContext destination)
	{
		_previous    = previous;
		_source      = source;
		_destination = destination;
	}

	public void Execute(EntityMigratorInput parameter)
	{
		var (logger, _) = parameter;
		var to     = _destination.Set<TTo>();
		var exists = KnownKeys<TFrom>.Default.Get(_source).IsSubsetOf(KnownKeys<TTo>.Default.Get(_destination));
		if (exists)
		{
			logger.LogInformation("Flatten {Set}: All source keys already present in destination (idempotent, no missing data)",
			                      to.GetType());
		}
		else
		{
			var cleared = to.ExecuteDelete();
			logger.LogInformation("Flatten {Set}: Cleared of {Count} entries", to.GetType(), cleared);
			_previous.Execute(parameter);
		}
	}

	public EntityTypeMapping Get() => _previous.Get();
}

sealed class KnownKeys<T> : ISelect<DbContext, ImmutableHashSet<object>> where T : class
{
	public static KnownKeys<T> Default { get; } = new();

	KnownKeys() {}

	public ImmutableHashSet<object> Get(DbContext parameter)
	{
		var entityType = parameter.Model.FindEntityType(A.Type<T>()).Verify();
		var key        = entityType.FindPrimaryKey().Verify();
		var names      = string.Join(',', key.Properties.Select(x => x.Name));
		var result     = parameter.Set<T>().Select(names).Cast<object>().ToImmutableHashSet();
		return result;
	}
}

public class BatchSelectorRegistration<T> : Instance<KeyValuePair<Type, IEntityMigrator>>
{
	protected BatchSelectorRegistration(IEntityMigrator instance) : base(new(typeof(T), instance)) {}
}

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

sealed class EntityMigratorSelector : IEntityMigratorSelector
{
	public static EntityMigratorSelector Default { get; } = new();

	EntityMigratorSelector()
		: this(Start.A.Generic(typeof(EntityMigrator<,>))
		            .Of.Type<IEntityMigrator>()
		            .WithParameterOf<DbContext>()
		            .AndOf<DbContext>()) {}

	readonly IGeneric<DbContext, DbContext, IEntityMigrator> _generic;

	public EntityMigratorSelector(IGeneric<DbContext, DbContext, IEntityMigrator> generic) => _generic = generic;

	public IEntityMigrator? Get(EntityMigratorSelectorInput parameter)
	{
		var (source, destination, result) = parameter;
		return result switch
		{
			ExactEntityComparisonResult(var from, var to)
				=> _generic.Get(from.ClrType, to.ClrType)(source, destination),
			MissingEntityComparisonResult => null,
			_ => throw new InvalidOperationException($"Could not find entity migrator for {result.From}")
		};
	}
}

public sealed class IdentityAwareEntityMigratorSelector : IEntityMigratorSelector
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
			var entityType = parameter.Destination.Model.FindEntityType(to).Verify();
			if (_identity.Get(entityType))
			{
				return new IdentityAwareEntityMigrator(previous, parameter.Destination, entityType);
			}
		}

		return previous;
	}
}

sealed class IdentityAwareEntityMigrator : IEntityMigrator
{
	readonly IEntityMigrator _previous;
	readonly DatabaseFacade  _database;
	readonly string          _template;

	public IdentityAwareEntityMigrator(IEntityMigrator previous, DbContext context, IEntityType type)
		: this(previous, context.Database, $"SET IDENTITY_INSERT [{type.GetSchema() ?? "dbo"}].[{type.GetTableName()}] {{0}}") {}

	public IdentityAwareEntityMigrator(IEntityMigrator previous, DatabaseFacade database, string template)
	{
		_previous      = previous;
		_database      = database;
		_template = template;
	}

	public void Execute(EntityMigratorInput parameter)
	{
		_database.ExecuteSqlRaw(_template.FormatWith("ON"));

		try
		{
			_previous.Execute(parameter);
		}
		finally
		{
			_database.ExecuteSqlRaw(_template.FormatWith("OFF"));
		}
	}

	public EntityTypeMapping Get() => _previous.Get();
}

sealed class IsIdentityEntity : ICondition<IEntityType>
{
	public static IsIdentityEntity Default { get; } = new();

	IsIdentityEntity() {}

	public bool Get(IEntityType type)
		=> type.FindPrimaryKey()?.Properties.Any(p => p.ValueGenerated == ValueGenerated.OnAdd) == true;
}