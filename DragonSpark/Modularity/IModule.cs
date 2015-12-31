using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup;
using System.Linq;

namespace DragonSpark.Modularity
{
	/// <summary>
	/// Defines the contract for the modules deployed in the application.
	/// </summary>
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
		readonly ISetupParameter setup;

		public SetupModuleCommand( IActivator activator, ISetupParameter setup )
		{
			this.activator = activator;
			this.setup = setup;
		}

		protected override void OnExecute( IMonitoredModule parameter )
		{
			var commands = activator.ActivateMany<IModuleCommand>( parameter.GetType().Assembly().ExportedTypes ).ToArray();
			commands.Apply<IModuleCommand>( parameter );
		}
	}

	public interface IModuleCommand : ICommand<IMonitoredModule>
	{}

	public abstract class ModuleCommand : Command<IMonitoredModule>, IModuleCommand
	{}

	public abstract class MonitoredModule<TCommand> : IMonitoredModule where TCommand : IModuleCommand
	{
		readonly TCommand command;

		protected MonitoredModule( IModuleMonitor moduleMonitor, TCommand command )
		{
			this.command = command;
			ModuleMonitor = moduleMonitor;
		}

		protected IModuleMonitor ModuleMonitor { get; }

		void IModule.Initialize()
		{
			OnInitialize();
		}

		protected virtual void OnInitialize()
		{
			ModuleMonitor.MarkAsLoaded( this );
		}

		void IMonitoredModule.Load()
		{
			OnLoad();
		}

		protected virtual void OnLoad()
		{
			command.Apply( this );
		}
	}
}