using DragonSpark.Configuration;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using Serilog;

namespace DragonSpark.Diagnostics
{
	public sealed class SystemLogger : LoggerBase
	{
		public static IConfigurableFactory<LoggerConfiguration, ILogger> Configurable { get; } = new SystemLogger();

		public static IScope<ILogger> Default { get; } = Configurable.ToCache().ToScope();
		SystemLogger() : base( DefaultSystemLoggerConfigurations.Default.Get() ) {}
	}
}