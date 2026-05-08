using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class IdentityPredicateBody<T> : ISelect<ImmutableHashSet<object>, Expression<Func<T, bool>>>
{
	readonly MethodInfo          _method;
	readonly ParameterExpression _parameter;
	readonly UnaryExpression     _expression;

	public IdentityPredicateBody(IEntityType type) : this(type, Expression.Parameter(typeof(T), "x")) {}

	public IdentityPredicateBody(IEntityType type, ParameterExpression expression)
		: this(expression, BuildKeyExpression(type, expression)) {}

	public IdentityPredicateBody(ParameterExpression parameter, MemberExpression member)
		: this(parameter, Expression.Convert(member, typeof(object))) {}

	public IdentityPredicateBody(ParameterExpression parameter, UnaryExpression expression)
		: this(ContainsMethod.Default, parameter, expression) {}

	public IdentityPredicateBody(MethodInfo method, ParameterExpression parameter, UnaryExpression expression)
	{
		_method     = method;
		_parameter  = parameter;
		_expression = expression;
	}

	public Expression<Func<T, bool>> Get(ImmutableHashSet<object> parameter)
	{
		var contains = Expression.Call(Expression.Constant(parameter), _method, _expression);
		var not      = Expression.Not(contains);
		return Expression.Lambda<Func<T, bool>>(not, _parameter);
	}

	// 🔥 NEW: no arrays, uses ValueTuple<object,object> for composite keys
	static UnaryExpression BuildKeyExpression(IEntityType type, ParameterExpression parameter)
	{
		var key        = type.FindPrimaryKey().Verify();
		var properties = key.Properties;

		Expression keyExpr;

		switch (properties.Count)
		{
			case 1:
				keyExpr = Expression.Property(parameter, properties[0].PropertyInfo.Verify());
				break;
			case 2:
			{
				var p0 = properties[0];
				var p1 = properties[1];

				var m0 = Expression.Property(parameter, p0.PropertyInfo.Verify());
				var m1 = Expression.Property(parameter, p1.PropertyInfo.Verify());

				var a0 = Expression.Convert(m0, typeof(object));
				var a1 = Expression.Convert(m1, typeof(object));

				var constructor = typeof(ValueTuple<object, object>).GetConstructor([typeof(object), typeof(object)])
				                  ?? throw new InvalidOperationException("Missing ValueTuple<object,object> ctor.");

				keyExpr = Expression.New(constructor, a0, a1);
				break;
			}
			default:
				throw new NotSupportedException("More than 2-part primary keys not supported yet.");
		}

		return Expression.Convert(keyExpr, typeof(object));
	}
}