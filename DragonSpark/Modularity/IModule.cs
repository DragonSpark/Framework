using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System.Linq;
using System.Windows.Input;

namespace DragonSpark.Modularity
{
	public interface IModule
	{
		void Initialize();
	}

	public interface IMonitoredModule : IModule
	{
		void Load();
	}

	public class SetupModuleCommand : ModuleCommand
	{
		readonly IActivator activator;
		
		public SetupModuleCommand( IActivator activator )
		{
			this.activator = activator;
		}

		protected override void OnExecute( IMonitoredModule parameter )
		{
			var commands = activator.ActivateMany<IModuleCommand>( parameter.GetType().Assembly().ExportedTypes ).ToArray();
			commands.ExecuteWith<IModuleCommand>( parameter );
		}
	}

	public interface IModuleCommand : ICommand<IMonitoredModule> {}

	public abstract class ModuleCommand : Command<IMonitoredModule>, IModuleCommand {}

	public abstract class MonitoredModule<TCommand> : IMonitoredModule where TCommand : ICommand
	{
		readonly IModuleMonitor moduleMonitor;
		readonly TCommand command;

		protected MonitoredModule( IModuleMonitor moduleMonitor, TCommand command )
		{
			this.moduleMonitor = moduleMonitor;
			this.command = command;
		}

		void IModule.Initialize() => OnInitialize();

		protected virtual void OnInitialize() => moduleMonitor.MarkAsLoaded( this );

		void IMonitoredModule.Load() => OnLoad();

		protected virtual void OnLoad() => command.ExecuteWith( (object)this );
	}
}