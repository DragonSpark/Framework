using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Sources
{
	public sealed class CachedFactoryBuilder<T> : AlterationBase<Func<T>>
	{
		public static IParameterizedSource<Func<T>, Func<T>> Default { get; } = new CachedFactoryBuilder<T>();
		CachedFactoryBuilder() {}

		public override Func<T> Get( Func<T> parameter ) => new SuppliedDeferredSource<T>( parameter ).Get;
	}
}