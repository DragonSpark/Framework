using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.IoC.Configuration;
using DragonSpark.Runtime;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC
{
	[ContentProperty( "Configurations" )]
	public class Launcher : UnityBootstrapper
	{
		class StorageLogger : ILoggerFacade
		{
			readonly IList<Tuple<string,Category,Microsoft.Practices.Prism.Logging.Priority>> storage = new List<Tuple<string, Category, Microsoft.Practices.Prism.Logging.Priority>>();

			public static StorageLogger Instance
			{
				get { return InstanceField; }
			}	static readonly StorageLogger InstanceField = new StorageLogger();

			void ILoggerFacade.Log( string message, Category category, Microsoft.Practices.Prism.Logging.Priority priority )
			{
				storage.Add( new Tuple<string, Category, Microsoft.Practices.Prism.Logging.Priority>( message, category, priority ) );
			}

			public ILoggerFacade Purge( ILoggerFacade logger )
			{
				storage.ToArray().Apply( x => logger.Log( x.Item1, x.Item2, x.Item3 ) );
				storage.Clear();
				return logger;
			}
		}

		protected override ILoggerFacade CreateLogger()
		{
			return StorageLogger.Instance;
		}

		protected override IUnityContainer CreateContainer()
		{
			var result = ServiceLocator.GetInstance<IUnityContainer>();
			return result;
		}

		public IServiceLocator ServiceLocator
		{
			get { return serviceLocator ?? ( serviceLocator = ResolveServiceLocator() ); }
		}	IServiceLocator serviceLocator;

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Designed to be disposed when application exits." )]
        protected virtual IServiceLocator ResolveServiceLocator()
		{
			var result = new UnityServiceLocator( Container ?? base.CreateContainer() );
			return result;
		}

		protected override void ConfigureContainer()
		{
			Container.AddNewExtensionIfNotPresent<DragonSparkExtension>();

			base.ConfigureContainer();

            var logger = Container.IsTypeRegistered( typeof(ILoggerFacade) ) ? Container.Resolve<ILoggerFacade>() : base.CreateLogger();
			Logger = StorageLogger.Instance.Purge( logger );

			Configurations.Apply( x =>
			{
			    x.Configure( Container );
			} );
		}

		public Collection<IContainerConfigurationCommand> Configurations
		{
			get { return configurations; }
		}	readonly Collection<IContainerConfigurationCommand> configurations = new Collection<IContainerConfigurationCommand>();
		
		protected override DependencyObject CreateShell()
		{
			return null;
		}
	}
}