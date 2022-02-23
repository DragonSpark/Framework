using DragonSpark.Presentation.Environment.Browser;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Presentation.Connections.Initialization;

sealed class Initialized : IInitialized
{
	readonly IIsInitialized        _initialized;
	readonly IInitializeConnection _initialize;
	readonly ILogger<Initialized>  _logger;

	public Initialized(IIsInitialized initialized, IInitializeConnection initialize, ILogger<Initialized> logger)
	{
		_initialized = initialized;
		_initialize  = initialize;
		_logger      = logger;
	}

	public bool Get(HttpContext parameter)
	{
		_logger.LogInformation("User-Agent: {UserAgent} - {IsBot}",
		                       UserAgent.Default.Get(parameter.Request), IsBot.Default.Get(parameter.Request));
		var result = _initialized.Get(parameter);
		if (!result)
		{
			_initialize.Execute(parameter);
		}

		return result;
	}
}