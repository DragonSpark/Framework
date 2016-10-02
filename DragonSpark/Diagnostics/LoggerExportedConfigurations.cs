using DragonSpark.Configuration;
using DragonSpark.Sources.Parameterized;
using Serilog;
using System.Linq;

namespace DragonSpark.Diagnostics
{
	public sealed class LoggerExportedConfigurations : ConfigurationSource<LoggerConfiguration>
	{
		public static LoggerExportedConfigurations Default { get; } = new LoggerExportedConfigurations();
		LoggerExportedConfigurations() : this( DefaultLoggerAlterations.Default.Get().ToArray() ) {}
		public LoggerExportedConfigurations( params IAlteration<LoggerConfiguration>[] configurators ) : base( configurators ) {}
	}
}