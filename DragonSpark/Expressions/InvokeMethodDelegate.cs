using System.Reflection;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Expressions
{
	class InvokeMethodDelegate<T> : InvocationFactoryBase<MethodInfo, T> where T : class
	{
		public static IParameterizedSource<MethodInfo, T> Default { get; } = new Cache<MethodInfo, T>( new InvokeMethodDelegate<T>().Get );
		InvokeMethodDelegate() : base( InvokeMethodExpressionFactory.Default.Create ) {}
	}
}