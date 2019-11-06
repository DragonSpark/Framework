using Serilog;
using Serilog.Configuration;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging
{
	sealed class LoggerDestructureSelector : Select<LoggerConfiguration, LoggerDestructuringConfiguration>
	{
		public static LoggerDestructureSelector Default { get; } = new LoggerDestructureSelector();

		LoggerDestructureSelector() : base(x => x.Destructure) {}
	}
}