using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public abstract class ReadFromCommandBase : ConfigureLoggerBase<LoggerSettingsConfiguration>
	{
		protected ReadFromCommandBase() : base( configuration => configuration.ReadFrom ) {}
	}
}