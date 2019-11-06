using Serilog.Events;
using DragonSpark.Model.Results;

namespace DragonSpark.Diagnostics.Logging
{
	sealed class DefaultLoggingLevel : Instance<LogEventLevel>
	{
		public static DefaultLoggingLevel Default { get; } = new DefaultLoggingLevel();

		DefaultLoggingLevel() : base(LogEventLevel.Information) {}
	}
}