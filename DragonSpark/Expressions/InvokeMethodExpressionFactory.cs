using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Expressions
{
	class InvokeMethodExpressionFactory : InvokeArrayFactoryBase<MethodInfo>
	{
		public static InvokeMethodExpressionFactory Default { get; } = new InvokeMethodExpressionFactory();
		InvokeMethodExpressionFactory() {}

		protected override Expression Apply( ExpressionBodyParameter<MethodInfo> parameter, Expression[] arguments ) => Expression.Call( parameter.Input, arguments );
	}
}