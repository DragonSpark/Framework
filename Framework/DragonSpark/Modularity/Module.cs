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
		public Module( IActivator activator, IModuleMonitor moduleMonitor, SetupContext context ) : base( activator, moduleMonitor, context )
		{}

		protected override IEnumerable<ICommand> DetermineCommands()
		{
			var result = Activator.Activate<TCommand>().Append().Cast<ICommand>().ToArray();
			return result;
		}
	}
}