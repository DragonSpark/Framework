using System;

namespace DragonSpark.Sources.Scopes
{
	public class ScopedSingleton<T> : Scope<T>
	{
		public ScopedSingleton( T instance ) : base( Factory.For( instance ) ) {}
		public ScopedSingleton( Func<T> defaultFactory ) : base( defaultFactory.Singleton() ) {}
	}
}