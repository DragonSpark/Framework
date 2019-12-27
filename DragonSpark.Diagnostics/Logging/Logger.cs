using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging.Configuration;
using DragonSpark.Model.Results;
using Serilog;

namespace DragonSpark.Diagnostics.Logging
{
	public sealed class Logger : Result<IPrimaryLogger>
	{
		public static Logger Default { get; } = new Logger();

		Logger() : base(Start.An.Instance(LoggingConfiguration.Default)
		                     .Assume()
		                     .In(Start.A.Result<LoggerConfiguration>().By.Instantiation())
		                     .Select(LoggerSelector.Default)
		                     .Then()
		                     .Activate<PrimaryLogger>()
		                     .Selector()) {}
	}
}