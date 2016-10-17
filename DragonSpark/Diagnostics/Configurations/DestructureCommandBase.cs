using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public abstract class DestructureCommandBase : ConfigureLoggerBase<LoggerDestructuringConfiguration>
	{
		protected DestructureCommandBase() : base( configuration => configuration.Destructure ) {}
	}
}