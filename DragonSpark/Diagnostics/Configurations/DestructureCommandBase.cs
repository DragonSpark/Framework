using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public abstract class DestructureCommandBase : LoggerConfigurationCommandBase<LoggerDestructuringConfiguration>
	{
		protected DestructureCommandBase() : base( configuration => configuration.Destructure ) {}
	}
}