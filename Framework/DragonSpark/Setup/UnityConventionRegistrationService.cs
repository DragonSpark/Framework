using DragonSpark.Extensions;
using DragonSpark.Logging;
using DragonSpark.Properties;
using Microsoft.Practices.Unity;
using System;

namespace DragonSpark.Setup
{
	public class UnityConventionRegistrationService : IConventionRegistrationService
	{

		public UnityConventionRegistrationService( IUnityContainer container, ILoggerFacade logger, ISingletonLocator locator )
		{
			Container = container;
			Logger = logger;
			Locator = locator;
		}

		protected IUnityContainer Container { get; }

		protected ILoggerFacade Logger { get; }

		public ISingletonLocator Locator { get; }

		protected virtual LifetimeManager DetermineLifetimeContainer<T>( Type type ) where T : LifetimeManager
		{
			var result = Activation.Activator.CreateInstance<LifetimeManager>( type.FromMetadata<LifetimeManagerAttribute, Type>( x => x.LifetimeManagerType ) ?? typeof(T) );
			Locator.Locate( type ).With( result.SetValue );
			return result;
		}

		public virtual void Register( ConventionRegistrationProfile profile )
		{
			profile.Candidates.WhereDecorated<RegisterAsAttribute>().Apply( item => item.Item2.AsType().With( type =>
			{
				Logger.Log( string.Format( Resources.UnityConventionRegistrationService_Registering, item.Item1.As, type ), Category.Debug, Logging.Priority.None );

				Container.RegisterType( item.Item1.As, type, item.Item1.Name, DetermineLifetimeContainer<ContainerControlledLifetimeManager>( type ) );
			} ) );
		}
	}
}