using DragonSpark.Model;
using System.Linq.Expressions;

namespace DragonSpark.Runtime.Invocation.Expressions
{
	sealed class Lambda<T> : Invocation0<Expression, ParameterExpression[], Expression<T>>
	{
		public static Lambda<T> Default { get; } = new Lambda<T>();

		Lambda() : this(Empty<ParameterExpression>.Array) {}

		public Lambda(params ParameterExpression[] parameters) : base(Expression.Lambda<T>, parameters) {}
	}
}