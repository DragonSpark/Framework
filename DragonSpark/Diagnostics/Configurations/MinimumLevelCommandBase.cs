using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public abstract class MinimumLevelCommandBase : LoggerConfigurationCommandBase<LoggerMinimumLevelConfiguration>
	{
		protected MinimumLevelCommandBase() : base( configuration => configuration.MinimumLevel ) {}
	}
}