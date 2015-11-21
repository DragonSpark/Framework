using DragonSpark.Activation;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.Unity;
using System;
using System.Linq;

namespace DragonSpark.Setup
{
	public class UnityConventionRegistrationService : IConventionRegistrationService
	{
		public UnityConventionRegistrationService( IUnityContainer container ) : this( container, container.Resolve<IActivator>() )
		{}

		public UnityConventionRegistrationService( IUnityContainer container, IActivator activator ) : this( container, activator, container.Resolve<ILogger>() )
		{}

		public UnityConventionRegistrationService( IUnityContainer container, IActivator activator, ILogger logger ) : this( container, activator, logger, SingletonLocator.Instance )
		{}

		public UnityConventionRegistrationService( IUnityContainer container, IActivator activator, ILogger logger, ISingletonLocator locator )
		{
			Activator = activator;
			Container = container;
			Logger = logger;
			Locator = locator;
		}

		protected IUnityContainer Container { get; }

		protected IActivator Activator { get; }

		protected ILogger Logger { get; }

		protected ISingletonLocator Locator { get; }

		protected virtual LifetimeManager DetermineLifetimeContainer<T>( Type type ) where T : LifetimeManager
		{
			var result = Activator.Activate<LifetimeManager>( type.FromMetadata<LifetimeManagerAttribute, Type>( x => x.LifetimeManagerType ) ?? typeof(T) );
			Locator.Locate( type ).With( result.SetValue );
			return result;
		}

		public virtual void Register( ConventionRegistrationProfile profile )
		{
			profile.Candidates.WhereDecorated<RegisterAsAttribute>().Apply( item => item.Item2.AsType().With( type =>
			{
				Logger.Information( string.Format( Resources.UnityConventionRegistrationService_Registering, item.Item1.As, type ) );

				Container.RegisterType( item.Item1.As ?? type.GetAllInterfaces().First(), type, item.Item1.Name, DetermineLifetimeContainer<ContainerControlledLifetimeManager>( type ) );
			} ) );
		}
	}
}