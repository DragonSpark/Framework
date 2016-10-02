using Serilog.Configuration;
using Serilog.Events;

namespace DragonSpark.Diagnostics.Configurations
{
	public class MinimumLevelIsCommand : MinimumLevelCommandBase
	{
		public MinimumLevelIsCommand() : this( LogEventLevel.Information ) {}

		public MinimumLevelIsCommand( LogEventLevel level )
		{
			Level = level;
		}

		public LogEventLevel Level { get; set; }

		protected override void Configure( LoggerMinimumLevelConfiguration configuration ) => configuration.Is( Level );
	}
}