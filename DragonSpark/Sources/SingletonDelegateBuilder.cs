using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Sources
{
	public sealed class SingletonDelegateBuilder<T> : AlterationBase<Func<T>>
	{
		public static IParameterizedSource<Func<T>, Func<T>> Default { get; } = new SingletonDelegateBuilder<T>();
		SingletonDelegateBuilder() {}

		public override Func<T> Get( Func<T> parameter ) => new SuppliedDeferredSource<T>( parameter ).Get;
	}
}