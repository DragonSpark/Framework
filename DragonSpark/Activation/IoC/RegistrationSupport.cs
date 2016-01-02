using DragonSpark.Extensions;
using DragonSpark.Runtime.Specifications;
using DragonSpark.Setup.Registration;
using DragonSpark.TypeSystem;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Activation.IoC
{
	[Register]
	public class RegistrationSupport
	{
		readonly IUnityContainer container;
		readonly ISpecification specification;
		readonly Assembly[] applicationAssemblies;

		public RegistrationSupport( IUnityContainer container, Assembly[] applicationAssemblies ) : this( container, AlwaysSpecification.Instance, applicationAssemblies )
		{}

		protected RegistrationSupport( IUnityContainer container, ISpecification specification, Assembly[] applicationAssemblies )
		{
			this.container = container;
			this.specification = specification;
			this.applicationAssemblies = applicationAssemblies;
		}

		public IUnityContainer Convention( object instance )
		{
			new[] { instance.Adapt().GetConventionCandidate( applicationAssemblies )/*, instance.GetType()*/ }.Distinct().Each( type =>
			{
				container.RegisterInstance( type, instance );
			} );

			return container;
		}

		IUnityContainer Check( Type type, Action apply )
		{
			specification.IsSatisfiedBy( new RegistrationSpecificationParameter( container, type ) ).IsTrue( apply );
			return container;
		}

		/*public IUnityContainer AllInterfaces( object instance )
		{
			return Check( instance.GetType(), () => ApplicationInterfaces( instance ).Each( y => container.RegisterInstance( y, instance ) ) );
		}

		Type[] ApplicationInterfaces( object instance )
		{
			var result = instance.Adapt().GetAllInterfaces().Where( x => applicationAssemblies.Contains( x.Assembly() ) ).ToArray();
			return result;
		}*/

		public IUnityContainer AllClasses( object instance )
		{
			return Check( instance.GetType(), () => instance.Adapt().GetEntireHierarchy().Each( y => container.RegisterInstance( y, instance ) ) );
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