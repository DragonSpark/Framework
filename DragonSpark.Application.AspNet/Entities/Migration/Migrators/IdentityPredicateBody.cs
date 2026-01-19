using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class IdentityPredicateBody<T> : ISelect<ImmutableHashSet<object>, Expression<Func<T, bool>>>
{
	readonly MethodInfo          _method;
	readonly ParameterExpression _parameter;
	readonly UnaryExpression     _expression;

	public IdentityPredicateBody(IEntityType type) : this(type, Expression.Parameter(typeof(T), "x")) {}

	public IdentityPredicateBody(IEntityType type, ParameterExpression expression)
		: this(expression,
		       Expression.Property(expression,
		                           type.FindPrimaryKey().Verify().Properties.Single().PropertyInfo.Verify())) {}

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
}