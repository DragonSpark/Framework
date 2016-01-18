using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.Activation.IoC
{
	public class TransientServiceRegistry : ServiceRegistry<TransientLifetimeManager>
	{
		public TransientServiceRegistry( IUnityContainer container, IMessageLogger logger, LifetimeManagerFactory<TransientLifetimeManager> factory ) : base( container, logger, factory ) {}
	}

	public class PersistentServiceRegistry : ServiceRegistry<ContainerControlledLifetimeManager>
	{
		public PersistentServiceRegistry( IUnityContainer container, IMessageLogger logger, LifetimeManagerFactory<ContainerControlledLifetimeManager> factory ) : base( container, logger, factory ) {}
	}

	public class ServiceRegistry<TLifetime> : ServiceRegistry where TLifetime : LifetimeManager
	{
		public ServiceRegistry( IUnityContainer container, IMessageLogger logger, LifetimeManagerFactory<TLifetime> factory ) : base( container, logger, factory ) {}
	}

	public class ServiceRegistry : IServiceRegistry
	{
		readonly IUnityContainer container;
		readonly IMessageLogger logger;
		readonly Func<Type, LifetimeManager> lifetimeFactory;

		[InjectionConstructor]
		public ServiceRegistry( IUnityContainer container, IMessageLogger logger, [Required]LifetimeManagerFactory factory ) : this( container, logger, factory.Create ) { }

		public ServiceRegistry( IUnityContainer container, Type lifetimeFactoryType ) : this( container, container.Logger(), t => container.Resolve<LifetimeManager>( lifetimeFactoryType ) ) { }

		public ServiceRegistry( IUnityContainer container, LifetimeManager lifetimeManager ) : this( container, container.Logger(), type => lifetimeManager ) { }

		protected ServiceRegistry( [Required]IUnityContainer container, [Required]IMessageLogger logger, [Required]Func<Type, LifetimeManager> lifetimeFactory )
		{
			this.container = container;
			this.logger = logger;
			this.lifetimeFactory = lifetimeFactory;
		}

		public void Register( MappingRegistrationParameter parameter )
		{
			var lifetimeManager = lifetimeFactory( parameter.MappedTo ) ?? new TransientLifetimeManager();
			container.RegisterType( parameter.Type, parameter.MappedTo, parameter.Name, lifetimeManager );
			logger.Information( string.Format( Resources.ServiceRegistry_Registering, parameter.Type, parameter.Name, lifetimeManager.GetType().FullName ) );
		}

		public void Register( InstanceRegistrationParameter parameter )
		{
			var to = parameter.Instance.GetType();
			var mapping = string.Concat( parameter.Type.FullName, to != parameter.Type ? $" -> {to.FullName}" : string.Empty );
			var lifetimeManager = lifetimeFactory( to ) ?? new ContainerControlledLifetimeManager();
			logger.Information( $"Registering Unity Instance: {mapping} ({lifetimeManager.GetType().FullName})" );
			container.RegisterInstance( parameter.Type, parameter.Name, parameter.Instance, lifetimeManager );
		}

		public void RegisterFactory( FactoryRegistrationParameter parameter )
		{
			var lifetimeManager = lifetimeFactory( parameter.Type ) ?? new TransientLifetimeManager();
			logger.Information( $"Registering Unity Factory: {parameter.Type} ({lifetimeManager.GetType().FullName})" );
			container.RegisterType( parameter.Type, parameter.Name, lifetimeManager, new InjectionFactory( x => parameter.Factory() ) );
		}
	}
}