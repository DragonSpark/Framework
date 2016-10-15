using DragonSpark.Specifications;
using System;

namespace DragonSpark.Activation.Location
{
	public class DecoratedActivator : ActivatorBase
	{
		public DecoratedActivator( Func<IActivator> provider ) : this( new DeferredSpecification<Type>( provider ),  provider.Delegate<object>() ) {}
		public DecoratedActivator( Func<Type, bool> specification, Func<Type, object> inner ) : this( new DelegatedSpecification<Type>( specification ), inner ) {}
		public DecoratedActivator( ISpecification<Type> specification, Func<Type, object> inner ) : base( specification, inner ) {}
	}
}