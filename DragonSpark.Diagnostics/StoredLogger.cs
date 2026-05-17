using DragonSpark.Model.Results;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace DragonSpark.Diagnostics;

sealed class StoredLogger : Stored<ILogger>
{
	public StoredLogger(IConfiguration configuration, Func<LoggerConfiguration, LoggerConfiguration> configure)
		: this(CurrentLogger.Default, configure, configuration) {}

	public StoredLogger(IMutable<ILogger?> store, Func<LoggerConfiguration, LoggerConfiguration> configure,
	                    IConfiguration configuration)
		: base(store, new CreateLogger(configure, configuration)) {}
}