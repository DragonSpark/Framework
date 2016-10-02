using System.Reflection;

namespace DragonSpark.Expressions
{
	class InvokeInstanceMethodDelegate<T> : InvocationFactoryBase<MethodInfo, T> where T : class
	{
		public InvokeInstanceMethodDelegate( object instance ) : base( new InvokeInstanceMethodExpressionFactory( instance ).Create ) {}
	}
}