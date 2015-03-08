using System.Collections.Generic;
using System.Linq;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Commands
{
	public class CommandMonitorContext
	{
		readonly IEnumerable<IMonitoredCommand> commands;
		readonly CommandMonitorOptions options;

		public CommandMonitorContext( IEnumerable<IMonitoredCommand> commands, CommandMonitorOptions options = null )
		{
			this.commands = commands.Select( x => x.WithDefaults() ).ToArray();
			this.options = options;
		}

		public IEnumerable<IMonitoredCommand> Commands
		{
			get { return commands; }
		}

		public CommandMonitorOptions Options
		{
			get { return options; }
		}
	}
}