using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public abstract class FilterCommandBase : LoggerConfigurationCommandBase<LoggerFilterConfiguration>
	{
		protected FilterCommandBase() : base( configuration => configuration.Filter ) {}
	}
}