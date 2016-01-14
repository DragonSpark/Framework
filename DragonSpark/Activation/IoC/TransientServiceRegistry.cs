using DragonSpark.Activation.FactoryModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using DragonSpark.Setup.Registration;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.Activation.IoC
{
	/*public class LifetimeManagerFactory : LifetimeManagerFactory<TransientLifetimeManager>
	{
		public LifetimeManagerFactory( IUnityContainer container ) : base( container ) { }
	}*/

	public class LifetimeManagerFactory<T> : FactoryBase<Type, LifetimeManager> where T : LifetimeManager
	{
		readonly IUnityContainer container;

		public LifetimeManagerFactory( [Required]IUnityContainer container )
		{
			this.container = container;
		}

		protected override LifetimeManager CreateItem( Type parameter )
		{
			var lifetimeManagerType = parameter.From<LifetimeManagerAttribute, Type>( x => x.LifetimeManagerType ) ?? typeof(T);
			var result = (LifetimeManager)container.Resolve( lifetimeManagerType );
			return result;
		}
	}

	public class TransientServiceRegistry : ServiceRegistry<TransientLifetimeManager>
	{
		public TransientServiceRegistry( IUnityContainer container, IMessageLogger logger, LifetimeManagerFactory<TransientLifetimeManager> factory ) : base( container, logger, factory ) {}
	}

	public class PersistingServiceRegistry : ServiceRegistry<ContainerControlledLifetimeManager>
	{
		public PersistingServiceRegistry( IUnityContainer container, IMessageLogger logger, LifetimeManagerFactory<ContainerControlledLifetimeManager> factory ) : base( container, logger, factory ) {}

		// public PersistingServiceRegistry( IUnityContainer container, IMessageLogger logger, Func<Type, LifetimeManager> lifetimeFactory ) : base( container, logger, lifetimeFactory ) {}
	}

	public class ServiceRegistry<TDefaultLifetimeManager> : ServiceRegistryBase where TDefaultLifetimeManager : LifetimeManager
	{
		public ServiceRegistry( [Required]IUnityContainer container, IMessageLogger logger, [Required]LifetimeManagerFactory<TDefaultLifetimeManager> factory ) : base( container, logger, factory.Create ) { }
	}

	public class ServiceRegistry : ServiceRegistryBase
	{
		public ServiceRegistry( IUnityContainer container, Type lifetimeFactoryType ) : this( container, (LifetimeManager)container.TryResolve( lifetimeFactoryType ) ) { }

		public ServiceRegistry( IUnityContainer container, LifetimeManager lifetimeManager ) : base( container, container.Logger(), type => lifetimeManager ) {}
	}

	public abstract class ServiceRegistryBase : IServiceRegistry
	{
		readonly IUnityContainer container;
		readonly IMessageLogger logger;
		readonly Func<Type, LifetimeManager> lifetimeFactory;

		protected ServiceRegistryBase( [Required]IUnityContainer container, [Required]IMessageLogger logger, [Required]Func<Type, LifetimeManager> lifetimeFactory )
		{
			this.container = container;
			this.logger = logger;
			this.lifetimeFactory = lifetimeFactory;
		}

		public void Register( MappingRegistrationParameter parameter )
		{
			var lifetimeManager = lifetimeFactory( parameter.MappedTo ) ?? new TransientLifetimeManager();
			logger.Information( string.Format( Resources.ServiceRegistry_Registering, parameter.Type, parameter.Name, lifetimeManager.GetType().FullName ) );
			container.RegisterType( parameter.Type, parameter.MappedTo, parameter.Name, lifetimeManager );
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
			container.RegisterType( parameter.Type, parameter.Name, lifetimeManager, new InjectionFactory( x => parameter.Factory ) );
		}
	}
}