using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using Serilog;

namespace DragonSpark.Diagnostics
{
	public sealed class LoggerExportedAlterations : AlterationsSource<LoggerConfiguration>
	{
		public static LoggerExportedAlterations Default { get; } = new LoggerExportedAlterations();
		LoggerExportedAlterations() : this( DefaultLoggerAlterations.Default.Unwrap() ) {}
		public LoggerExportedAlterations( params IAlteration<LoggerConfiguration>[] configurators ) : base( configurators ) {}
	}
}