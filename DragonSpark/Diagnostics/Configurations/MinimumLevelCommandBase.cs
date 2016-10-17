using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public abstract class MinimumLevelCommandBase : ConfigureLoggerBase<LoggerMinimumLevelConfiguration>
	{
		protected MinimumLevelCommandBase() : base( configuration => configuration.MinimumLevel ) {}
	}
}