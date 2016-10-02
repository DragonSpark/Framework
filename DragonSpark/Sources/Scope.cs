using DragonSpark.Aspects;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;

namespace DragonSpark.Sources
{
	public class Scope<T> : SourceBase<T>, IScope<T>, IDisposable
	{
		readonly ICache<Func<object, T>> factories = new Cache<Func<object, T>>();
		readonly IAssignableSource<object> scope;
		readonly IAssignableSource<Func<object, T>> defaultFactory = new SuppliedSource<Func<object, T>>();

		public Scope() : this( () => default(T) ) {}

		public Scope( Func<T> defaultFactory ) : this( defaultFactory.Wrap() ) {}

		public Scope( Func<object, T> defaultFactory ) : this( new ScopeContext(), defaultFactory ) {}

		protected Scope( IAssignableSource<object> scope, Func<object, T> defaultFactory )
		{
			this.scope = scope;
			this.defaultFactory.Assign( defaultFactory );
		}

		public virtual void Assign( Func<T> item ) => factories.SetOrClear( scope.Get(), item?.Wrap() );

		public virtual void Assign( Func<object, T> item )
		{
			defaultFactory.Assign( item );

			factories.Remove( scope.Get() );
		}

		public override T Get()
		{
			var context = scope.Get();
			var factory = factories.Get( context ) ?? defaultFactory.Get();
			var result = factory( context );
			return result;
		}

		public void Assign( ISource item ) => scope.Assign( item );
		
		~Scope()
		{
			Dispose( false );
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		[Freeze]
		void Dispose( bool disposing ) => OnDispose( disposing );

		protected virtual void OnDispose( bool disposing ) {}
	}
}