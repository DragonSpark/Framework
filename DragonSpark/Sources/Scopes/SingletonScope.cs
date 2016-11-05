using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Collections.Generic;

namespace DragonSpark.Sources.Scopes
{
	/*public abstract class ScopedSource<T> : DelegatedSource<T>, IScope<T>
	{
		readonly IScope<T> scope;

		protected ScopedSource( IScope<T> scope ) : base( scope.Get )
		{
			this.scope = scope;
			scope.Assign( new Func<T>( Create ).Scoped );
		}

		protected abstract T Create();

		public void Assign( ISource item ) => scope.Assign( item );
		public void Assign( Func<object, T> item ) => scope.Assign( item );
		public void Assign( Func<T> item ) => scope.Assign( item );
	}*/

	public class SingletonScope<T> : Scope<T>
	{
		public SingletonScope() {}
		public SingletonScope( T instance ) : base( Factory.For( instance ) ) {}
		public SingletonScope( Func<T> defaultFactory ) : base( Caches.Create( defaultFactory ).Get ) {}
		public SingletonScope( Func<object, T> defaultFactory ) : base( Caches.Create( defaultFactory ).Get ) {}

		public override void Assign( Func<T> item ) => base.Assign( item.ToSingleton() );

		public override void Assign( Func<object, T> item ) => base.Assign( Caches.Create( item ).Get );
	}

	public class ItemsScope<T> : DecoratedItemsSource<T>
	{
		public ItemsScope( IScope<IEnumerable<T>> scope ) : base( scope )
		{
			Scope = scope;
		}

		public IScope<IEnumerable<T>> Scope { get; }
	}
}