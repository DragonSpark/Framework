using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Reflection
{
	public static class Defaults
	{
		public static Func<Type, ParameterExpression> Parameter { get; } = Expression.Parameter;

		public static Func<ParameterExpression, Type, Expression> ExpressionZip { get; }
			= (expression, type) => type.GetTypeInfo().IsAssignableFrom(expression.Type)
				                        ? expression
				                        : Expression.Convert(expression, type);
	}
}