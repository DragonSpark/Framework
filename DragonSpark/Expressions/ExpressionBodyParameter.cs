using System.Linq.Expressions;

namespace DragonSpark.Expressions
{
	public struct ExpressionBodyParameter<T>
	{
		public ExpressionBodyParameter( T input, ParameterExpression parameter )
		{
			Input = input;
			Parameter = parameter;
		}

		public T Input { get; }
		public ParameterExpression Parameter { get; }
	}
}