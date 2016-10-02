using DragonSpark.Expressions;
using DragonSpark.Sources.Parameterized.Caching;
using System.Reflection;

namespace DragonSpark.Activation
{
	class ConstructorDelegateFactory<T> :  InvocationFactoryBase<ConstructorInfo, T> where T : class
	{
		public static ICache<ConstructorInfo, T> Default { get; } = new Cache<ConstructorInfo, T>( new ConstructorDelegateFactory<T>().Get );
		ConstructorDelegateFactory() : base( ActivateFromArrayExpression.Default.Create ) {}
	}
}