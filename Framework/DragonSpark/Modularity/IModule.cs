using DragonSpark.Extensions;
using DragonSpark.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Modularity
{
	/// <summary>
	/// Defines the contract for the modules deployed in the application.
	/// </summary>
	public interface IModule
	{
		void Initialize();

		void Load();
	}

	public class Module : IModule
	{
		readonly IModuleMonitor moduleMonitor;
		readonly SetupContext context;

		public Module( IModuleMonitor moduleMonitor, SetupContext context )
		{
			this.moduleMonitor = moduleMonitor;
			this.context = context;
		}

		void IModule.Initialize()
		{
			Initialize();

			moduleMonitor.MarkAsLoaded( this );
		}

		protected virtual void Initialize()
		{}

		void IModule.Load()
		{
			Load();
		}

		protected virtual void Load()
		{
			var commands = DetermineCommands();
			commands.Where( command => command.CanExecute( context ) ).Apply( x => x.Execute( context ) );
		}

		protected virtual IEnumerable<ICommand> DetermineCommands()
		{
			var result = GetType().GetTypeInfo().Assembly.ExportedTypes.Where( typeof(ICommand).IsAssignableFrom ).Select( Activator.CreateInstance<ICommand> ).ToArray();
			return result;
		}
	}
}