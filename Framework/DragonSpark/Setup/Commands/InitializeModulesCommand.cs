using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Modularity;
using DragonSpark.Properties;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public class InitializeModulesCommand : SetupCommand
	{
		[Activate, Required]
		public IModuleMonitor Monitor { [return: Required]get; set; }

		[Activate, Required]
		public IMessageLogger MessageLogger { [return: Required]get; set; }

		[Activate, Required]
		public IModuleManager Manager { [return: Required]get; set; }

		protected override async void Execute( SetupContext context )
		{
			MessageLogger.Information( Resources.InitializingModules, Priority.Low );
			Manager.Run();

			MessageLogger.Information( Resources.LoadingModules, Priority.Low );
			await Monitor.Load();
			MessageLogger.Information( Resources.ModulesLoaded, Priority.Low );
		}

		/*static IModuleManager Resolve( IUnityContainer container )
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
		}*/
	}
}