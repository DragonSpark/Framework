using System;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Specifications;
using DragonSpark.Setup.Registration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Activation.IoC
{
	[Register]
	public class RegistrationSupport
	{
		readonly IUnityContainer container;
		readonly ISpecification specification;

		public RegistrationSupport( IUnityContainer container ) : this( container, AlwaysSpecification.Instance )
		{}

		protected RegistrationSupport( IUnityContainer container, ISpecification specification )
		{
			this.container = container;
			this.specification = specification;
		}

		public IUnityContainer Convention( object instance )
		{
			return Check( instance.GetType(), () => container.RegisterInstance( instance.Adapt().GetConventionCandidate(), instance ) );
		}

		IUnityContainer Check( Type type, Action apply )
		{
			specification.IsSatisfiedBy( new RegistrationSpecificationParameter( container, type ) ).IsTrue( apply );
			return container;
		}

		public IUnityContainer AllInterfaces( object instance )
		{
			return Check( instance.GetType(), () => instance.Adapt().GetAllInterfaces().Each( y => container.RegisterInstance( y, instance ) ) );
		}

		public IUnityContainer AllClasses( object instance )
		{
			return Check( instance.GetType(), () => instance.Adapt().GetAllHierarchy().Each( y => container.RegisterInstance( y, instance ) ) );
		}

		public IUnityContainer Mapping<TInterface, TImplementation>( LifetimeManager manager = null ) where TImplementation : TInterface
		{
			return Check( typeof(TInterface), () => container.RegisterType<TInterface, TImplementation>( manager ?? new TransientLifetimeManager() ) );
		}

		/*public IUnityContainer Instance( Type type, object instance, LifetimeManager manager = null )
		{
			return Instance( type, () => instance, manager );
		}

		public IUnityContainer Instance( Type type, Func<object> instance, LifetimeManager manager = null )
		{
			return Check( type, () => container.RegisterInstance( type, instance(), manager ?? new ContainerControlledLifetimeManager() ) );
		}*/

		public IUnityContainer Instance<TInterface>( TInterface instance, LifetimeManager manager = null )
		{
			return Instance( () => instance, manager );
		}

		public IUnityContainer Instance<TInterface>( Func<TInterface> instance, LifetimeManager manager = null )
		{
			return Check( typeof(TInterface), () => container.RegisterInstance( typeof(TInterface), instance(), manager ?? new ContainerControlledLifetimeManager() ) );
		}
	}
}