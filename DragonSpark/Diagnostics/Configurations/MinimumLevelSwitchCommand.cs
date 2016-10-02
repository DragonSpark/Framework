using PostSharp.Patterns.Contracts;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace DragonSpark.Diagnostics.Configurations
{
	public class MinimumLevelSwitchCommand : MinimumLevelCommandBase
	{
		public MinimumLevelSwitchCommand() : this( LogEventLevel.Information ) {}

		public MinimumLevelSwitchCommand( LogEventLevel level ) : this( new LoggingLevelSwitch( level ) ) {}

		public MinimumLevelSwitchCommand( LoggingLevelSwitch controller )
		{
			Controller = controller;
		}

		[Required]
		public LoggingLevelSwitch Controller { [return: Required]get; set; }

		protected override void Configure( LoggerMinimumLevelConfiguration configuration ) => configuration.ControlledBy( Controller );
	}
}