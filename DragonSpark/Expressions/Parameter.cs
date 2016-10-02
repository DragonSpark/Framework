using System.Linq.Expressions;

namespace DragonSpark.Expressions
{
	public static class Parameter
	{
		public static ParameterExpression Create<T>( string name = "parameter" ) => Expression.Parameter( typeof(T), name );

		public static ParameterExpression Default { get; } = Create<object[]>();
	}
}