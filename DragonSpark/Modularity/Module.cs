using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DragonSpark.Modularity
{
	public class Module<TCommand> : Module where TCommand : ICommand
	{
		public Module( IActivator activator, IModuleMonitor moduleMonitor, ISetupParameter parameter ) : base( activator, moduleMonitor, parameter )
		{}

		protected override IEnumerable<ICommand> DetermineCommands()
		{
			var result = Activator.Activate<TCommand>().ToItem().Cast<ICommand>().ToArray();
			return result;
		}
	}
}