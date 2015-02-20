using System.Configuration;
using System.Diagnostics;
using DragonSpark.Activation.IoC;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Unity.Configuration;
using IExceptionHandler = DragonSpark.Diagnostics.IExceptionHandler;

namespace DragonSpark.Application
{
	public class LoggerFacade : ILoggerFacade
	{
		public void Log( string message, Category category, Microsoft.Practices.Prism.Logging.Priority priority )
		{
			Trace.WriteLine( string.Format( "Message: {0} - Category: {1} - Priority: {2}.", message, category, priority ) );
		}
	}

	// [ContentProperty( "Configurations" )]
	public class Launcher : UnityBootstrapper
	{
		class StorageLogger : ILoggerFacade
		{
			readonly IList<Tuple<string, Category, Microsoft.Practices.Prism.Logging.Priority>> storage = new List<Tuple<string, Category, Microsoft.Practices.Prism.Logging.Priority>>();

			public static StorageLogger Instance
			{
				get { return InstanceField; }
			}

			static readonly StorageLogger InstanceField = new StorageLogger();

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
			var locator = CreateServiceLocator();
			var result = locator.GetInstance<IUnityContainer>();
			return result;
		}

		protected virtual IServiceLocator CreateServiceLocator()
		{
			var container = Container ?? base.CreateContainer();
			var configured = container.FromConfiguration();
			var result = new Activation.IoC.ServiceLocator( configured );
			return result;
		}

		protected override void ConfigureContainer()
		{
			base.ConfigureContainer();

			Configurations.Apply( x => x.WithDefaults().Configure( Container ) );

			var logger = Container.IsRegistered<ILoggerFacade>() ? Container.Resolve<ILoggerFacade>() : base.CreateLogger();
			Logger = StorageLogger.Instance.Purge( logger );
		}

		public Collection<IContainerConfigurationCommand> Configurations
		{
			get { return configurations; }
		}

		readonly Collection<IContainerConfigurationCommand> configurations = new Collection<IContainerConfigurationCommand>();

		protected override DependencyObject CreateShell()
		{
			return null;
		}
	}

	public static class UnityContainerExtensions
	{
		public static IUnityContainer FromConfiguration( this IUnityContainer container )
		{
			ConfigurationManager.GetSection( "unity" ).As<UnityConfigurationSection>( x => x.Containers.Any().IsTrue( () => container.LoadConfiguration() ) );
			return container;
		}
	}

	public class ExceptionHandler : IExceptionHandler
	{
		public const string DefaultExceptionPolicy = "Default Exception Policy";
		readonly ExceptionManager manager;
		readonly string policyName;

		public ExceptionHandler( ExceptionManager manager, string policyName = DefaultExceptionPolicy )
		{
			this.manager = manager;
			this.policyName = policyName;
		}

		public virtual ExceptionHandlingResult Handle( Exception exception )
		{
			Exception resultingException;
			var rethrow = manager.HandleException( exception, policyName, out resultingException );
			var result = new ExceptionHandlingResult( rethrow, resultingException ?? exception );
			return result;
		}
	}
}