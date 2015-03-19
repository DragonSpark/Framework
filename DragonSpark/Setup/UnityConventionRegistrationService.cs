using System;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.Unity;
using Prism.Logging;

namespace DragonSpark.Setup
{
	public class UnityConventionRegistrationService : IConventionRegistrationService
	{
		readonly IUnityContainer container;
		readonly ILoggerFacade logger;
		
		public UnityConventionRegistrationService( IUnityContainer container, ILoggerFacade logger )
		{
			this.container = container;
			this.logger = logger;
		}

		protected IUnityContainer Container
		{
			get { return container; }
		}

		protected ILoggerFacade Logger
		{
			get { return logger; }
		}

		protected virtual LifetimeManager DetermineLifetimeContainer<T>( Type type ) where T : LifetimeManager, new()
		{
			var result = type.FromMetadata<LifetimeManagerAttribute, LifetimeManager>( x => Activation.Activator.CreateInstance<LifetimeManager>( x.LifetimeManagerType ) ) ?? new T();
			return result;
		}

		public virtual void Register( ConventionRegistrationProfile profile )
		{
			profile.Candidates.WhereDecorated<RegisterAsAttribute>().Apply( item => item.Item2.AsType().With( type =>
			{
				logger.Log( string.Format( Resources.UnityConventionRegistrationService_Registering, item.Item1.As, type ), Category.Debug, Prism.Logging.Priority.None );
				container.RegisterType( item.Item1.As, type, DetermineLifetimeContainer<ContainerControlledLifetimeManager>( type ) );
			} ) );
		}
	}
}