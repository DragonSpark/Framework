using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DragonSpark.Runtime
{
	public static class DynamicExpression
	{
		#region Static Members
		public static System.Linq.Expressions.Expression Parse(Type resultType, string expression, params object[] values)
		{
			var parser = new ExpressionParser(null, expression, values);
			return parser.Parse(resultType);
		}

		public static LambdaExpression ParseLambda(Type itType, Type resultType, string expression, params object[] values)
		{
			return ParseLambda(new[] { System.Linq.Expressions.Expression.Parameter(itType, "") }, resultType, expression, values);
		}

		public static LambdaExpression ParseLambda(ParameterExpression[] parameters, Type resultType, string expression, params object[] values)
		{
			var parser = new ExpressionParser(parameters, expression, values);
			return System.Linq.Expressions.Expression.Lambda(parser.Parse(resultType), parameters);
		}

		public static System.Linq.Expressions.Expression<Func<T, S>> ParseLambda<T, S>(string expression, params object[] values)
		{
			return (System.Linq.Expressions.Expression<Func<T, S>>)ParseLambda(typeof(T), typeof(S), expression, values);
		}

		public static Type CreateClass(params DynamicProperty[] properties)
		{
			return ClassFactory.Instance.GetDynamicClass(properties);
		}

		public static Type CreateClass(IEnumerable<DynamicProperty> properties)
		{
			return ClassFactory.Instance.GetDynamicClass(properties);
		}
		#endregion
	}
}