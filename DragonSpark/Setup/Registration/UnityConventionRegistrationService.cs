using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Activation.IoC;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup.Registration
{
	public class LifetimeManagerFactory : LifetimeManagerFactory<TransientLifetimeManager>
	{
		public LifetimeManagerFactory( IActivator activator ) : this( activator, SingletonLocator.Instance )
		{}

		public LifetimeManagerFactory( IActivator activator, ISingletonLocator locator ) : base( activator, locator )
		{}
	}

	public class LifetimeManagerFactory<T> : ActivateFactory<LifetimeManager> where T : LifetimeManager
	{
		readonly ISingletonLocator locator;

		/*public LifetimeManagerFactory() : this( Activation.Activator.Current )
		{}*/

		public LifetimeManagerFactory( IActivator activator ) : this( activator, SingletonLocator.Instance )
		{}

		public LifetimeManagerFactory( IActivator activator, ISingletonLocator locator ) : base( activator, new LifetimeFactoryParameterCoercer( activator, typeof(T) ) )
		{
			this.locator = locator;
		}

		protected override LifetimeManager Activate( ActivateParameter parameter )
		{
			var result = base.Activate( parameter ).With( manager =>
			{
				locator.Locate( parameter.Type ).With( manager.SetValue );
			} );;
			return result;
		}
	}

	public class UnityConventionRegistrationService : IConventionRegistrationService
	{
		public UnityConventionRegistrationService( IUnityContainer container, ILogger logger  ) : this( container, logger, new LifetimeManagerFactory<ContainerControlledLifetimeManager>( container.Resolve<IActivator>() ) )
		{}

		public UnityConventionRegistrationService( IUnityContainer container, ILogger logger, IFactory<ActivateParameter, LifetimeManager> factory  )
		{
			Container = container;
			Logger = logger;
			Factory = factory;
		}

		protected IUnityContainer Container { get; }
		protected ILogger Logger { get; }
		protected IFactory<ActivateParameter, LifetimeManager> Factory { get; }

		public virtual void Register( ConventionRegistrationProfile profile )
		{
			var registry = new ServiceRegistry( Container, Logger, Factory );
			
			profile.Candidates
				.AsTypeInfos()
				.WhereDecorated<RegistrationBaseAttribute>()
				.Each( item => item.Item1.Registration.Register( registry, item.Item2.AsType() ) );
		}
	}
}