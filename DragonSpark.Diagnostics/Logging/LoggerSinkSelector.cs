using Serilog;
using Serilog.Configuration;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging
{
	sealed class LoggerSinkSelector : Select<LoggerConfiguration, LoggerSinkConfiguration>
	{
		public static LoggerSinkSelector Default { get; } = new LoggerSinkSelector();

		LoggerSinkSelector() : base(x => x.WriteTo) {}
	}
}