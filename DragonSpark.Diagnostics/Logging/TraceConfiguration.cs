using Serilog;
using Serilog.Configuration;
using DragonSpark.Diagnostics.Logging.Configuration;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging
{
	sealed class TraceConfiguration : Select<LoggerSinkConfiguration, LoggerConfiguration>, ILoggingSinkConfiguration
	{
		public static TraceConfiguration Default { get; } = new TraceConfiguration();

		TraceConfiguration() : base(x => x.Trace()) {}
	}
}