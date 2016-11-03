using System;

namespace DragonSpark.Sources.Scopes
{
	public class SingletonScope<T> : Scope<T>
	{
		public SingletonScope( T instance ) : base( Factory.For( instance ) ) {}
		public SingletonScope( Func<T> defaultFactory ) : base( defaultFactory.ToGlobalSingleton() ) {}
	}
}