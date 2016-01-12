using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using DragonSpark.Runtime.Specifications;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;
using System.Reflection;
using DragonSpark.Setup.Registration;
using DragonSpark.TypeSystem;

namespace DragonSpark.Activation.IoC
{
	[Register.Type]
	public class RegistrationSupport
	{
		readonly IUnityContainer container;
		readonly IMessageLogger logger;
		readonly ISpecification specification;
		readonly ImplementedInterfaceFromConventionLocator factory;
		
		public RegistrationSupport( IUnityContainer container, Assembly[] applicationAssemblies ) : this( container, applicationAssemblies, AlwaysSpecification.Instance ) {}

		protected RegistrationSupport( IUnityContainer container, Assembly[] applicationAssemblies, ISpecification specification ) : this( container, container.Logger(), specification, new ImplementedInterfaceFromConventionLocator( applicationAssemblies ) ) {}

		protected RegistrationSupport( [Required]IUnityContainer container, [Required]IMessageLogger logger, [Required]ISpecification specification, [Required]ImplementedInterfaceFromConventionLocator factory )
		{
			this.container = container;
			this.logger = logger;
			this.specification = specification;
			this.factory = factory;
		}

		public IUnityContainer Convention( [Required]object instance ) => 
			factory.Create( instance.GetType() ).With( type => Instance( type, instance ) );

		IUnityContainer Check( Type type, Action apply )
		{
			specification.IsSatisfiedBy( new RegistrationSpecificationParameter( container, type ) ).IsTrue( apply );
			return container;
		}

		public IUnityContainer AllClasses( object instance )
		{
			instance.Adapt().GetEntireHierarchy().Each( y => Instance( y, instance ) );
			return container;
		}

		public IUnityContainer Mapping<TInterface, TImplementation>( string name = null, LifetimeManager manager = null ) where TImplementation : TInterface => Mapping( typeof(TInterface), typeof(TImplementation), name, manager );

		public IUnityContainer Mapping( Type from, Type to, string name = null, LifetimeManager manager = null ) => Check( @from, () => Map( @from, to, name, manager ) );

		public IUnityContainer Instance( Type type, object instance, string name = null, LifetimeManager manager = null ) => Instance( type, () => instance, name, manager );

		public IUnityContainer Instance( Type type, Func<object> instance, string name = null, LifetimeManager manager = null ) => Check( type, () => RegisterInstance( type, instance(), name, manager ) );

		public IUnityContainer Instance<TInterface>( TInterface instance, string name = null, LifetimeManager manager = null ) => Instance( typeof(TInterface), instance, name, manager );

		public IUnityContainer Instance<TInterface>( Func<TInterface> instance, string name = null, LifetimeManager manager = null ) => Instance( typeof(TInterface), () => instance(), name, manager );

		IUnityContainer Map( Type from, Type to, string name = null, LifetimeManager manager = null )
		{
			var lifetimeManager = manager ?? new TransientLifetimeManager();
			logger.Information( string.Format( Resources.ServiceRegistry_Registering, @from, to, lifetimeManager.GetType().FullName ) );
			return container.RegisterType( @from, to, name, lifetimeManager );
		}

		IUnityContainer RegisterInstance( Type type, object instance, string name = null, LifetimeManager manager = null )
		{
			var to = instance.GetType();
			var mapping = string.Concat( type.FullName, to != type ? $" -> {to.FullName}" : string.Empty );
			logger.Information( $"Registering Unity Instance: {mapping}" );
			return container.RegisterInstance( type, name, instance, manager ?? new ContainerControlledLifetimeManager() );
		}
	}
}