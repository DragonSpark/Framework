using System;
using DragonSpark.Activation;
using DragonSpark.Activation.Build;
using DragonSpark.Activation.IoC;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup.Registration
{
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

		protected override LifetimeManager Activate( Type qualified, ActivateParameter parameter )
		{
			var result = base.Activate( qualified, parameter ).With( manager =>
			{
				locator.Locate( parameter.Type ).With( manager.SetValue );
			} );;
			return result;
		}

		protected override Type DetermineType( ActivateParameter parameter )
		{
			return base.DetermineType( parameter ).FromMetadata<LifetimeManagerAttribute, Type>( x => x.LifetimeManagerType );
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
				.Each( item => item.Item1.Register( registry, item.Item2.AsType() ) );
		}
	}
}