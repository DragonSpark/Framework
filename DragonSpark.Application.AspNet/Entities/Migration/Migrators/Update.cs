using DragonSpark.Application.AspNet.Entities.Migration.Identity;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Array = System.Array;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Update<TFrom, TTo> : DestinationBase<TFrom, TTo> where TFrom : class where TTo : class
{
	public Update(IMap map) : base(LocateAwareInstance<TFrom, TTo>.Default, map) {}
}

// TODO V2
sealed class LocateAwareInstance<TFrom, TTo> : IInstance<TFrom, TTo> where TTo : class where TFrom : class
{
	public static LocateAwareInstance<TFrom, TTo> Default { get; } = new();

	LocateAwareInstance() : this(EntityMaps<TFrom, TTo>.Default, Activate<TFrom, TTo>.Default, Keys.Default) {}

	readonly IEntityMaps<TFrom, TTo>      _maps;
	readonly IInstance<TFrom, TTo>        _previous;
	readonly ISelect<EntityEntry, object> _key;

	public LocateAwareInstance(IEntityMaps<TFrom, TTo> maps, IInstance<TFrom, TTo> previous,
	                           ISelect<EntityEntry, object> key)
	{
		_maps     = maps;
		_previous = previous;
		_key      = key;
	}

	public async ValueTask<TTo> Get(Stop<MappingInput<TFrom>> parameter)
	{
		var ((source, destination, page, from), stop) = parameter;

		var map    = await _maps.Get(source).Get(destination).Off(new(page.Open(), stop));
		var key    = _key.Get(source.Entry(from));
		var exists = map.TryGet(key, out var existing);
		return exists ? existing : await _previous.Off(parameter);
	}
}

public interface IEntityMaps<TFrom, TTo> : ISelect<DbContext, IEntityMap<TFrom, TTo>>;

sealed class EntityMaps<TFrom, TTo> : ReferenceValueStore<DbContext, IEntityMap<TFrom, TTo>>, IEntityMaps<TFrom, TTo>
	where TFrom : class where TTo : class
{
	public static EntityMaps<TFrom, TTo> Default { get; } = new();

	EntityMaps() : base(x => new EntityMap<TFrom, TTo>(x)) {}
}

public interface IEntityMap<TFrom, TTo>
	: ISelect<DbContext, IStopAware<IReadOnlyCollection<TFrom>, IConditional<object, TTo>>>;

sealed class EntityMap<TFrom, TTo>
	: ReferenceValueStore<DbContext, IStopAware<IReadOnlyCollection<TFrom>, IConditional<object, TTo>>>,
	  IEntityMap<TFrom, TTo>
	where TFrom : class where TTo : class
{
	public EntityMap(DbContext source)
		: base(x => new ComposeEntityMap<TFrom, TTo>(source, x).AsReferenceStoring()) {}
}

sealed class ComposeEntityMap<TFrom, TTo> : IStopAware<IReadOnlyCollection<TFrom>, IConditional<object, TTo>>
	where TFrom : class where TTo : class
{
	readonly Func<object, object> _source;
	readonly DbSet<TTo>           _destination;
	readonly Func<TTo, object>    _existing;

	public ComposeEntityMap(DbContext source, DbContext destination) : this(source, destination, Keys.Default.Get) {}

	public ComposeEntityMap(DbContext source, DbContext destination, Func<EntityEntry, object> keys)
		: this(Start.A.Selection<object, EntityEntry>(source.Entry).Select(keys).Get, destination.Set<TTo>(), keys) {}

	public ComposeEntityMap(Func<object, object> source, DbSet<TTo> destination, Func<EntityEntry, object> keys)
		: this(source, destination, Start.A.Selection<TTo, EntityEntry>(destination.Entry).Select(keys).Get) {}

	public ComposeEntityMap(Func<object, object> source, DbSet<TTo> destination, Func<TTo, object> existing)
	{
		_source      = source;
		_destination = destination;
		_existing    = existing;
	}

	public async ValueTask<IConditional<object, TTo>> Get(Stop<IReadOnlyCollection<TFrom>> parameter)
	{
		var (subject, stop) = parameter;
		var keys     = subject.Select(_source).ToArray();
		var where    = ComposeWhere<TTo>.Default.Get(new(_destination.EntityType, keys));
		var existing = await _destination.Where(where).ToArrayAsync(stop).Off();
		var result   = existing.ToDictionary(_existing, StructuralEqualityComparer<TFrom, TTo>.Default).ToTable();
		return result;
	}
}

sealed class StructuralEqualityComparer<TFrom, TTo> : IEqualityComparer<object>
{
	public static StructuralEqualityComparer<TFrom, TTo> Default { get; } = new();

	StructuralEqualityComparer() : this(StructuralComparisons.StructuralEqualityComparer) {}

	readonly IEqualityComparer _previous;

	public StructuralEqualityComparer(IEqualityComparer previous) => _previous = previous;

	bool IEqualityComparer<object>.Equals(object? x, object? y) => _previous.Equals(x, y);

	public int GetHashCode(object obj) => _previous.GetHashCode(obj);
}

public readonly record struct ComposeContainsInput(IEntityType Metadada, Array<object> Keys);

sealed class ComposeWhere<T> : ISelect<ComposeContainsInput, Expression<Func<T, bool>>>
{
	public static ComposeWhere<T> Default { get; } = new();

	ComposeWhere() : this(ComposeContains<T>.Default, ComposeCompositeContains<T>.Default) {}

	readonly ISelect<ComposeContainsInput, Expression<Func<T, bool>>> _single;
	readonly ISelect<ComposeContainsInput, Expression<Func<T, bool>>> _composite;

	public ComposeWhere(ISelect<ComposeContainsInput, Expression<Func<T, bool>>> single,
	                    ISelect<ComposeContainsInput, Expression<Func<T, bool>>> composite)
	{
		_single    = single;
		_composite = composite;
	}

	public Expression<Func<T, bool>> Get(ComposeContainsInput parameter)
	{
		var select = parameter.Metadada.FindPrimaryKey().Verify().Properties.Count == 1 ? _single : _composite;
		return select.Get(parameter);
	}
}

sealed class ComposeContains<T> : ISelect<ComposeContainsInput, Expression<Func<T, bool>>>
{
	public static ComposeContains<T> Default { get; } = new();

	ComposeContains() : this(ComposeKeySelector.Default, Cast.Default, Expression.Parameter(typeof(T), "y")) {}

	readonly ISelect<IEntityType, LambdaExpression> _key;
	readonly ISelect<CastInput, Array>              _cast;
	readonly ParameterExpression                    _y;

	public ComposeContains(ISelect<IEntityType, LambdaExpression> key, ISelect<CastInput, Array> cast,
	                       ParameterExpression y)
	{
		_key  = key;
		_cast = cast;
		_y    = y;
	}

	public Expression<Func<T, bool>> Get(ComposeContainsInput parameter)
	{
		var (metadata, keys) = parameter;
		var key     = _key.Get(metadata);
		var x       = key.Parameters[0];
		var body    = new ReplaceParameterVisitor(x, _y).Visit(key.Body);
		var objects = _cast.Get(new(keys, key.ReturnType));
		var contains = Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), [key.ReturnType],
		                               Expression.Constant(objects), body);

		return Expression.Lambda<Func<T, bool>>(contains, _y);
	}

	sealed class ReplaceParameterVisitor : ExpressionVisitor
	{
		readonly ParameterExpression _from;
		readonly Expression          _to;

		public ReplaceParameterVisitor(ParameterExpression from, Expression to)
		{
			_from = from;
			_to   = to;
		}

		protected override Expression VisitParameter(ParameterExpression node) => node == _from ? _to : node;
	}
}

sealed class ComposeCompositeContains<T> : ISelect<ComposeContainsInput, Expression<Func<T, bool>>>
{
	public static ComposeCompositeContains<T> Default { get; } = new();

	readonly ParameterExpression _entity;

	ComposeCompositeContains() : this(Expression.Parameter(typeof(T), "x")) {}

	public ComposeCompositeContains(ParameterExpression entity) => _entity = entity;

	public Expression<Func<T, bool>> Get(ComposeContainsInput parameter)
	{
		var (metadata, input) = parameter;

		var         properties = metadata.FindPrimaryKey().Verify().Properties;
		Expression? body       = null;

		foreach (var row in input)
		{
			if (row is object[] values && values.Length == properties.Count)
			{
				Expression? and = null;

				for (var i = 0; i < properties.Count; i++)
				{
					var left     = Expression.Property(_entity, properties[i].PropertyInfo.Verify());
					var type     = properties[i].ClrType;
					var constant = Expression.Constant(Convert.ChangeType(values[i], type), type);
					var equal    = Expression.Equal(left, constant);
					and = and is null ? equal : Expression.AndAlso(and, equal);
				}

				body = body is null ? and : Expression.OrElse(body, and!);
			}
		}

		body ??= Expression.Constant(false);

		return Expression.Lambda<Func<T, bool>>(body, _entity);
	}
}

public readonly record struct ComposeCompositeContainerBodyInput(
	ParameterExpression Key,
	IEntityType Metadata,
	Array<object> Keys);

sealed class ComposeCompositeContainerBody : ISelect<ComposeCompositeContainerBodyInput, MethodCallExpression>
{
	public static ComposeCompositeContainerBody Default { get; } = new();

	ComposeCompositeContainerBody() : this(CompositeKeyType.Default, CompositeEqualityChain.Default, Cast.Default) {}

	readonly ISelect<IReadOnlyList<IProperty>, Type>          _type;
	readonly ISelect<CompositeEqualityChainInput, Expression> _chain;
	readonly ISelect<CastInput, Array>                        _cast;

	public ComposeCompositeContainerBody(ISelect<IReadOnlyList<IProperty>, Type> type,
	                                     ISelect<CompositeEqualityChainInput, Expression> chain,
	                                     ISelect<CastInput, Array> cast)
	{
		_type  = type;
		_chain = chain;
		_cast  = cast;
	}

	public MethodCallExpression Get(ComposeCompositeContainerBodyInput parameter)
	{
		var (x, metadata, input) = parameter;
		var properties = metadata.FindPrimaryKey().Verify().Properties;
		var type       = _type.Get(properties);
		var y          = Expression.Parameter(type, "y");
		var chain      = _chain.Get(new(properties, x, y));
		var body       = Expression.Lambda(chain, y);
		var keys       = _cast.Get(new(input, type)).AsTo<Array, Expression>(z => Expression.Constant(z, z.GetType()));
		return Expression.Call(typeof(Enumerable), nameof(Enumerable.Any), [type], keys, body);
	}
}

public readonly record struct CompositeEqualityChainInput(
	IReadOnlyList<IProperty> Properties,
	ParameterExpression Entity,
	ParameterExpression Key);

sealed class CompositeEqualityChain : ISelect<CompositeEqualityChainInput, Expression>
{
	public static CompositeEqualityChain Default { get; } = new();

	CompositeEqualityChain() {}

	public Expression Get(CompositeEqualityChainInput parameter)
	{
		var (properties, x, y) = parameter;

		using var fields = y.Type.GetFields()
		                    .Where(f => f.Name.StartsWith("Item"))
		                    .OrderBy(f => f.Name)
		                    .AsValueEnumerable()
		                    .ToArray(ArrayPool<FieldInfo>.Shared);

		Expression? result = null;

		for (var i = 0; i < properties.Count; i++)
		{
			var left  = Expression.Property(x, properties[i].PropertyInfo.Verify());
			var right = Expression.Field(y, fields.Memory.Span[i]);
			var equal = Expression.Equal(left, right);
			result = result is null ? equal : Expression.AndAlso(result, equal);
		}

		return result.Verify();
	}
}

sealed class CompositeKeyType : ISelect<IReadOnlyList<IProperty>, Type>
{
	public static CompositeKeyType Default { get; } = new();

	CompositeKeyType() {}

	public Type Get(IReadOnlyList<IProperty> parameter)
	{
		var types = parameter.Select(p => p.ClrType).ToArray();
		return parameter.Count switch
		{
			2 => typeof(ValueTuple<,>).MakeGenericType(types),
			3 => typeof(ValueTuple<,,>).MakeGenericType(types),
			_ => throw new NotSupportedException("Composite key too large")
		};
	}
}

sealed class ComposeKeySelector : ISelect<IEntityType, LambdaExpression>
{
	public static ComposeKeySelector Default { get; } = new();

	ComposeKeySelector() {}

	public LambdaExpression Get(IEntityType parameter)
	{
		var key = parameter.FindPrimaryKey().Verify().Properties;
		var x   = Expression.Parameter(parameter.ClrType, "x");
		return Expression.Lambda(Expression.Property(x, key[0].PropertyInfo.Verify()), x);
	}
}

sealed class ComposeCompositeKeySelector : ISelect<IEntityType, LambdaExpression>
{
	public static ComposeCompositeKeySelector Default { get; } = new();

	ComposeCompositeKeySelector() {}

	public LambdaExpression Get(IEntityType parameter)
	{
		var key = parameter.FindPrimaryKey().Verify().Properties;
		var p   = Expression.Parameter(parameter.ClrType, "e");

		var ctor = key.Count switch
		{
			2 => typeof(ValueTuple<,>).MakeGenericType(key[0].ClrType, key[1].ClrType).GetConstructors().Single(),
			3 => typeof(ValueTuple<,,>).MakeGenericType(key[0].ClrType, key[1].ClrType, key[2].ClrType)
			                           .GetConstructors()
			                           .Single(),
			_ => throw new NotSupportedException("Only up to 3 PK columns supported.")
		};

		var args = key.Select(x => Expression.Property(p, x.PropertyInfo.Verify())).Cast<Expression>().ToArray();
		var body = Expression.New(ctor, args);

		return Expression.Lambda(body, p);
	}
}