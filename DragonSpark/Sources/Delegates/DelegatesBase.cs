using DragonSpark.Activation;
using DragonSpark.Expressions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using DragonSpark.TypeSystem.Generics;
using System;

namespace DragonSpark.Sources.Delegates
{
	public abstract class DelegatesBase : CacheWithImplementedFactoryBase<Type, Delegate>
	{
		protected DelegatesBase( IActivator source, string name ) : this( source.ToDelegate(), Common.Assigned, name ) {}
		protected DelegatesBase( Func<Type, object> locator, ISpecification<Type> specification, string name ) : base( specification )
		{
			Locator = locator;
			Methods = GetType().Adapt().GenericFactoryMethods[ name ];
		}

		protected Func<Type, object> Locator { get; }
		protected IGenericMethodContext<Invoke> Methods { get; }
	}
}