using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Expressions
{
	class InvokeInstanceMethodExpressionFactory : InvokeArrayFactoryBase<MethodInfo>
	{
		readonly object instance;
		public InvokeInstanceMethodExpressionFactory( object instance )
		{
			this.instance = instance;
		}

		protected override Expression Apply( ExpressionBodyParameter<MethodInfo> parameter, Expression[] arguments ) => Expression.Call( Expression.Constant( instance ), parameter.Input, arguments );
	}
}