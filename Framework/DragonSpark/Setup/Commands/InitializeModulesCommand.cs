using System;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Properties;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup.Commands
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
				context.Logger.Information( Resources.InitializingModules, Priority.Low);
				InitializeModules( context, manager );

				context.Logger.Information( Resources.LoadingModules, Priority.Low);
				await Monitor.Load();
				context.Logger.Information( Resources.ModulesLoaded, Priority.Low);
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