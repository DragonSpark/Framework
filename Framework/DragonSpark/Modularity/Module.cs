using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Setup;

namespace DragonSpark.Modularity
{
	public class Module<TCommand> : Module where TCommand : ICommand
	{
		public Module( IModuleMonitor moduleMonitor, SetupContext context ) : base( moduleMonitor, context )
		{}

		protected override IEnumerable<ICommand> DetermineCommands()
		{
			var result = Activator.Create<TCommand>().Append().Cast<ICommand>().ToArray();
			return result;
		}
	}
}