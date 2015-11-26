using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.Unity;
using System;

namespace DragonSpark.Setup
{
	/*public class ConventionRegistrationContext
	{
		public ConventionRegistrationContext( IUnityContainer container ) : this( container,  )
		{
		}

		public ConventionRegistrationContext( IUnityContainer container, IServiceRegistry registry ) : this( container, registry, container.DetermineLogger() )
		{}

		public ConventionRegistrationContext( IUnityContainer container, IServiceRegistry registry, ILogger logger )
		{
			Container = container;
			Registry = registry;
			Logger = logger;
		}

		public IUnityContainer Container { get; }

		public IServiceRegistry Registry { get; set; }

		public ILogger Logger { get; }
	}*/

	public class LifetimeManagerFactory<T> : LifetimeManagerFactory where T : LifetimeManager
	{
		public LifetimeManagerFactory()
		{}

		public LifetimeManagerFactory( IActivator activator ) : base( activator )
		{}

		public LifetimeManagerFactory( IActivator activator, ISingletonLocator locator ) : base( activator, locator )
		{}

		protected override Type DetermineType( Type parameter )
		{
			return base.DetermineType( parameter ) ?? typeof(T);
		}
	}

	public class LifetimeManagerFactory : ActivateFactory<LifetimeManager>
	{
		readonly ISingletonLocator locator;

		public LifetimeManagerFactory() : this( Activation.Activator.Current )
		{}

		public LifetimeManagerFactory( IActivator activator ) : this( activator, SingletonLocator.Instance )
		{}

		public LifetimeManagerFactory( IActivator activator, ISingletonLocator locator ) : base( activator )
		{
			this.locator = locator;
		}

		protected sealed override LifetimeManager CreateItem( Type parameter )
		{
			var result = DetermineType( parameter ).Transform( Activator.Activate<LifetimeManager> ).With( manager =>
			{
				locator.Locate( parameter ).With( manager.SetValue );
			} );;
			return result;
		}

		protected virtual Type DetermineType( Type parameter )
		{
			var result = parameter.FromMetadata<LifetimeManagerAttribute, Type>( x => x.LifetimeManagerType );
			return result;
		}
	}

	public class UnityConventionRegistrationService : IConventionRegistrationService
	{
		public UnityConventionRegistrationService( IUnityContainer container, ILogger logger  ) : this( container, logger, new LifetimeManagerFactory<ContainerControlledLifetimeManager>( container.Resolve<IActivator>() ) )
		{}

		public UnityConventionRegistrationService( IUnityContainer container, ILogger logger, IFactory<Type, LifetimeManager> factory  )
		{
			Container = container;
			Logger = logger;
			Factory = factory;
		}

		protected IUnityContainer Container { get; }
		protected ILogger Logger { get; }
		protected IFactory<Type, LifetimeManager> Factory { get; }

		public virtual void Register( ConventionRegistrationProfile profile )
		{
			var registry = new ServiceRegistry( Container, Logger, Factory );
			
			profile.Candidates
				.AsTypeInfos()
				.WhereDecorated<RegistrationBaseAttribute>()
				.Apply( item => item.Item1.Register( registry, item.Item2.AsType() ) );
		}
	}
}