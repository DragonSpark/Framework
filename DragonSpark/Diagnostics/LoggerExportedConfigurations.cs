using DragonSpark.Configuration;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using Serilog;

namespace DragonSpark.Diagnostics
{
	public sealed class LoggerExportedConfigurations : ConfigurationSource<LoggerConfiguration>
	{
		public static LoggerExportedConfigurations Default { get; } = new LoggerExportedConfigurations();
		LoggerExportedConfigurations() : this( DefaultLoggerAlterations.Default.Unwrap() ) {}
		public LoggerExportedConfigurations( params IAlteration<LoggerConfiguration>[] configurators ) : base( configurators ) {}
	}
}