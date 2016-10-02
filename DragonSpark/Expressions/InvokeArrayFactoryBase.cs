using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Expressions
{
	abstract class InvokeArrayFactoryBase<T> where T : MethodBase
	{
		public virtual Expression Create( ExpressionBodyParameter<T> parameter )
		{
			var array = ArgumentsArrayExpressionFactory.Default.Get( new ArgumentsArrayParameter( parameter.Input, parameter.Parameter ) );
			var result = Apply( parameter, array );
			return result;
		}

		protected abstract Expression Apply( ExpressionBodyParameter<T> parameter, Expression[] arguments );
	}
}