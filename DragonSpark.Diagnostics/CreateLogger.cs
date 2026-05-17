using DragonSpark.Model.Results;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace DragonSpark.Diagnostics;

sealed class CreateLogger : IResult<ILogger>
{
	readonly Func<LoggerConfiguration, LoggerConfiguration> _configure;
	readonly IConfiguration                                 _configuration;

	public CreateLogger(Func<LoggerConfiguration, LoggerConfiguration> configure, IConfiguration configuration)
	{
		_configure     = configure;
		_configuration = configuration;
	}

	public ILogger Get()
	{
		var configuration = new LoggerConfiguration().ReadFrom.Configuration(_configuration);
		var configured    = _configure(configuration);
		return configured.CreateLogger();
	}
}