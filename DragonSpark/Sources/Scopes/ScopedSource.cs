using System;

namespace DragonSpark.Sources.Scopes
{
	public abstract class ScopedSource<T> : DelegatedSource<T>
	{
		protected ScopedSource( Func<T> factory ) : this( factory.Create() ) {}
		protected ScopedSource( IScope<T> scope ) : base( scope.ToDelegate() ) {}
	}
}