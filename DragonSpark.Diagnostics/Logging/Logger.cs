using Serilog;
using DragonSpark.Diagnostics.Logging.Configuration;
using DragonSpark.Model.Results;

namespace DragonSpark.Diagnostics.Logging
{
	public sealed class Logger : Result<IPrimaryLogger>
	{
		public static Logger Default { get; } = new Logger();

		Logger() : base(LoggingConfiguration.Default
		                                    .Select(x => x.Get(new LoggerConfiguration()))
		                                    .Select(LoggerSelector.Default)
		                                    .Then()
		                                    .Activate<PrimaryLogger>()
		                                    .Selector()) {}
	}
}