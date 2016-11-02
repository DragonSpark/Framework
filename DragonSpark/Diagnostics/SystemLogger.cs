using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using Serilog;

namespace DragonSpark.Diagnostics
{
	public sealed class SystemLogger : LoggerBase
	{
		/*public static IConfigurationProvisionedFactory<LoggerConfiguration, ILogger> Configurable { get; } = new SystemLogger();*/

		public static IScope<ILogger> Default { get; } = new SystemLogger().ToCache().ToExecutionScope();
		SystemLogger() : base( DefaultSystemLoggerConfigurations.Default.Get() ) {}
	}
}