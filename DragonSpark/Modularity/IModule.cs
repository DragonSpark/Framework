using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

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

	public class Module : IMonitoredModule
	{
		public Module( IActivator activator, IModuleMonitor moduleMonitor, SetupContext context )
		{
			Activator = activator;
			ModuleMonitor = moduleMonitor;
			Context = context;
		}

		protected IActivator Activator { get; }
		protected IModuleMonitor ModuleMonitor { get; }
		protected SetupContext Context { get; }

		void IModule.Initialize()
		{
			Initialize();

			ModuleMonitor.MarkAsLoaded( this );
		}

		protected virtual void Initialize()
		{}

		void IMonitoredModule.Load()
		{
			Load();
		}

		protected virtual void Load()
		{
			var commands = DetermineCommands();
			commands.Where( command => command.CanExecute( Context ) ).Each( x => x.Execute( Context ) );
		}

		protected virtual IEnumerable<ICommand> DetermineCommands()
		{
			var result = Activator.ActivateMany<ICommand>( GetType().Assembly().ExportedTypes ).ToArray();
			return result;
		}
	}
}