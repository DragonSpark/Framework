using Serilog;
using Serilog.Configuration;
using Serilog.Core;

namespace DragonSpark.Diagnostics.Logging.Configuration
{
	sealed class SinkConfiguration : ILoggingSinkConfiguration
	{
		readonly ILogEventSink _sink;

		public SinkConfiguration(ILogEventSink sink) => _sink = sink;

		public LoggerConfiguration Get(LoggerSinkConfiguration parameter) => parameter.Sink(_sink);
	}
}