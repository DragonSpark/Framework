using System;

namespace DragonSpark.Sources.Scopes
{
	public abstract class ScopedSourceBase<T> : DelegatedSource<T>
	{
		protected ScopedSourceBase( Func<T> factory ) : this( factory.Create() ) {}
		protected ScopedSourceBase( IScope<T> scope ) : base( scope.ToDelegate() ) {}
	}

	public abstract class ScopedSourceWithImplementedFactoryBase<T> : ScopedSourceBase<T>
	{
		protected ScopedSourceWithImplementedFactoryBase() : this( new Scope<T>() ) {}

		protected ScopedSourceWithImplementedFactoryBase( IScope<T> scope ) : base( scope )
		{
			scope.Assign( new Func<T>( Create ).GlobalCache() );
		}

		protected abstract T Create();
	}
}