using System.Linq.Expressions;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Expressions
{
	class ArgumentsArrayExpressionFactory : ParameterizedSourceBase<ArgumentsArrayParameter, Expression[]>
	{
		public static ArgumentsArrayExpressionFactory Default { get; } = new ArgumentsArrayExpressionFactory();
		ArgumentsArrayExpressionFactory() {}

		public override Expression[] Get( ArgumentsArrayParameter parameter )
		{
			var types = parameter.Method.GetParameterTypes();
			var result = new Expression[types.Length];
			for ( var i = 0; i < types.Length; i++ )
			{
				var index = Expression.ArrayIndex( parameter.Parameter, Expression.Constant( i ) );
				result[i] = Expression.Convert( index, types[i] );
			}
			return result;
		}
	}
}