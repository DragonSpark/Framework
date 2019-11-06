using Serilog;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging
{
	public sealed class LoggerSelector : Select<LoggerConfiguration, ILogger>
	{
		public static LoggerSelector Default { get; } = new LoggerSelector();

		LoggerSelector() : base(x => x.CreateLogger()) {}
	}
}