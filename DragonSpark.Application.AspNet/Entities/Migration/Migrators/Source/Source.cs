using DragonSpark.Application.AspNet.Entities.Migration.Identity;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;

public sealed class Source<T> : ISource<T>
{
	public static Source<T> Default { get; } = new();

	Source() : this(SourceQueries<T>.Default) {}

	readonly ISelect<IModel, IAlteration<IQueryable<T>>> _queries;

	public Source(ISelect<IModel, IAlteration<IQueryable<T>>> queries) => _queries = queries;

	public IQueryable<T> Get(Stop<SourceInput<T>> parameter)
	{
		var ((_, (source, _), queryable, _, _, _), _) = parameter;
		return _queries.Get(source.Model).Get(queryable);
	}
}

sealed class SourceQueries<T> : ReferenceValueStore<IModel, IAlteration<IQueryable<T>>>
{
	public static SourceQueries<T> Default { get; } = new();

	SourceQueries() : base(x => A.Selection(new SourceQuery<T>(x).Then().Stores().New()).Then().Out()) {}
}

sealed class SourceQuery<T> : IAlteration<IQueryable<T>>
{
	readonly IReadOnlyList<IProperty>? _key;
	readonly MethodInfo                _property;
	readonly Type                      _type;

	public SourceQuery(IModel model)
		: this(model.FindEntityType(A.Type<T>())?.FindPrimaryKey()?.Properties, PropertyMethod.Default,
		       typeof(Queryable)) {}

	public SourceQuery(IReadOnlyList<IProperty>? key, MethodInfo property, Type type)
	{
		_key      = key;
		_property = property;
		_type     = type;
	}

	public IQueryable<T> Get(IQueryable<T> parameter)
	{
		if (_key != null && _key.Count != 0)
		{
			var x       = Expression.Parameter(typeof(T), "x");
			var current = parameter.Expression;

			for (var i = 0; i < _key.Count; i++)
			{
				var metadata = _key[i];
				var access = metadata.PropertyInfo is {} p
					             ? (Expression)Expression.MakeMemberAccess(x, p)
					             : Expression.Call(null, _property.MakeGenericMethod(metadata.ClrType), x,
					                               Expression.Constant(metadata.Name));
				var order = Expression.Lambda(access, x);
				var types = new[] { typeof(T), metadata.ClrType };
				var quote = Expression.Quote(order);
				var name  = i == 0 ? nameof(Queryable.OrderBy) : nameof(Queryable.ThenBy);
				current = Expression.Call(_type, name, types, current, quote);
			}

			return parameter.Provider.CreateQuery<T>(current);
		}

		return parameter;
	}
}