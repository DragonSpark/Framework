using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Modularity;
using DragonSpark.Properties;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public class InitializeModulesCommand : SetupCommandBase
	{
		[Locate, Required]
		public IModuleMonitor Monitor { [return: Required]get; set; }

		[Locate, Required]
		public IMessageLogger MessageLogger { [return: Required]get; set; }

		[Locate, Required]
		public IModuleManager Manager { [return: Required]get; set; }

		[AmbientValue, Required]
		public ITaskMonitor Tasks { [return: Required]get; set; }

		protected override void OnExecute( object parameter )
		{
			MessageLogger.Information( Resources.InitializingModules, Priority.Low );
			Manager.Run();

			MessageLogger.Information( Resources.LoadingModules, Priority.Low );
			Tasks.Monitor( Monitor.Load().ContinueWith( task =>
			{
				MessageLogger.Information( Resources.ModulesLoaded, Priority.Low );
			} ) );
		}
	}
}