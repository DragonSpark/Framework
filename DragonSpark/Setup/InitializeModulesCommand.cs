using System;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Logging;
using DragonSpark.Modularity;
using DragonSpark.Properties;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup
{
	public class InitializeModulesCommand : SetupCommand
	{
		[Activate]
		public IModuleMonitor Monitor { get; set; }

		protected override async void Execute( SetupContext context )
		{
			var manager = DetermineManager( context );
			if ( manager != null )
			{
				context.Logger.Log( Resources.InitializingModules, Category.Debug, Logging.Priority.Low);
				InitializeModules( context, manager );

				context.Logger.Log( Resources.LoadingModules, Category.Debug, Logging.Priority.Low);
				await Monitor.Load();
				context.Logger.Log( Resources.ModulesLoaded, Category.Debug, Logging.Priority.Low);
			}
		}

		protected virtual void InitializeModules( SetupContext context, IModuleManager manager )
		{
			manager.Run();
		}

		protected virtual IModuleManager DetermineManager( SetupContext context )
		{
			var container = context.Container();
			var result = container.IsRegistered<IModuleManager>() ? Resolve( container ) : null;
			return result;
		}

		static IModuleManager Resolve( IUnityContainer container )
		{
			try
			{
				return container.Resolve<IModuleManager>();
			}
			catch ( ResolutionFailedException ex )
			{
				if ( ex.Message.Contains( nameof(IModuleCatalog) ) )
				{
					throw new InvalidOperationException( Resources.NullModuleCatalogException );
				}

				throw;
			}
		}
	}
}