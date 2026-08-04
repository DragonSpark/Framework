using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.Extensions.Configuration;
using Serilog;
using ILogger = Serilog.ILogger;

namespace DragonSpark.Diagnostics;

sealed class CreateLogger : Result<ILogger>
{
	public CreateLogger(IConfiguration configuration)
		: this(ApplyConfiguration.Default.Parameter(new(configuration)).Subject) {}

	public CreateLogger(LoggerConfiguration configuration) : base(configuration.CreateLogger) {}
}