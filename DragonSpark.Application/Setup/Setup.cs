using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Input;
using System.Windows.Markup;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Prism;
using Prism.Logging;
using Prism.Modularity;
using Prism.Unity;
using ServiceLocator = DragonSpark.Activation.IoC.ServiceLocator;

namespace DragonSpark.Application.Setup
{
	public class Setup<TLoggingFacade> : Setup<TLoggingFacade, ModuleCatalog> where TLoggingFacade : ILoggerFacade, new()
	{}

	[ContentProperty( "Commands" )]
	public class Setup<TLoggingFacade, TModuleCatalog> : Prism.Setup<TLoggingFacade, TModuleCatalog> where TLoggingFacade : ILoggerFacade, new() where TModuleCatalog : IModuleCatalog, new()
	{
		public override void Run( object arguments = null, bool runWithDefaultConfiguration = true )
		{
			Events.Publish<SetupEvent, SetupStatus>( SetupStatus.Configuring );

			base.Run( runWithDefaultConfiguration );

			Events.Publish<SetupEvent, SetupStatus>( SetupStatus.Configured );
		}

		protected override IEnumerable<ICommand> DetermineCommands( SetupContext context )
		{
			var result = base.DetermineCommands( context ).Select( command => command.WithDefaults() );
			return result;
		}

		/*class StorageLogger : ILoggerFacade
		{
			readonly IList<Tuple<string, Category, Prism.Logging.Priority>> storage = new List<Tuple<string, Category, Prism.Logging.Priority>>();

			public static StorageLogger Instance
			{
				get { return InstanceField; }
			}

			static readonly StorageLogger InstanceField = new StorageLogger();

			void ILoggerFacade.Log( string message, Category category, Prism.Logging.Priority priority )
			{
				storage.Add( new Tuple<string, Category, Prism.Logging.Priority>( message, category, priority ) );
			}

			public ILoggerFacade Purge( ILoggerFacade logger )
			{
				storage.ToArray().Apply( x => logger.Log( x.Item1, x.Item2, x.Item3 ) );
				storage.Clear();
				return logger;
			}
		}*/
		
		/*protected override ILoggerFacade CreateLogger()
		{
			return StorageLogger.Instance;
		}*/

		/**/

		/*protected override void ConfigureContainer()
		{
			base.ConfigureContainer();

			Events.Publish<SetupEvent, SetupStatus>( SetupStatus.Configuring );

			Configurations.Apply( x =>
			{
				var command = x.WithDefaults();
				command.Configure( Container );
			} );

			var logger = Container.TryResolve<ILoggerFacade>() ?? base.CreateLogger();
			Logger = StorageLogger.Instance.Purge( logger );
			
			Events.Publish<SetupEvent, SetupStatus>( SetupStatus.Configured );
		}*/
	}

	public class SetupUnityCommand : Prism.Unity.SetupUnityCommand
	{
		protected override IUnityContainer CreateContainer()
		{
			var locator = CreateServiceLocator();
			var result = locator.GetInstance<IUnityContainer>();
			return result;
		}

		protected virtual IServiceLocator CreateServiceLocator()
		{
			var container = base.CreateContainer();
			var result = new ServiceLocator( container );
			return result;
		}
	}

	public class SetupUnityFromConfigurationCommand : SetupCommand
	{
		protected override void Execute( SetupContext context )
		{
			ConfigurationManager.GetSection( "unity" ).As<UnityConfigurationSection>( x => x.Containers.Any().IsTrue( () => context.Container().LoadConfiguration() ) );
		}
	}
}