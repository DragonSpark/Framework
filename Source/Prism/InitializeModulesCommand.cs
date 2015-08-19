using Prism.Logging;
using Prism.Modularity;
using Prism.Properties;

namespace Prism
{
	public abstract class InitializeModulesCommand : SetupCommand
	{
		protected override void Execute( SetupContext context )
		{
			var manager = DetermineManager( context );
			if ( manager != null )
			{
				context.Logger.Log(Resources.InitializingModules, Category.Debug, Priority.Low);
				this.InitializeModules( context, manager );
			}
		}

		protected virtual void InitializeModules( SetupContext context, IModuleManager manager )
		{
			manager.Run();
		}

		protected abstract IModuleManager DetermineManager( SetupContext context );
	}
}