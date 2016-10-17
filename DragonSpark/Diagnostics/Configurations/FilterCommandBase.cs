using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public abstract class FilterCommandBase : ConfigureLoggerBase<LoggerFilterConfiguration>
	{
		protected FilterCommandBase() : base( configuration => configuration.Filter ) {}
	}
}