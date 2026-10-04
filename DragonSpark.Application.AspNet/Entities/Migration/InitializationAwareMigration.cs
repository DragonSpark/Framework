using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class InitializationAwareMigration<T> : IMigration where T : DbContext
{
	readonly IMigration             _previous;
	readonly InitializeMigration<T> _initialize;
	readonly ILogger                _logger;

	public InitializationAwareMigration(IMigration previous, InitializeMigration<T> initialize,
	                                    ILogger<InitializationAwareMigration<T>> logger)
	{
		_previous   = previous;
		_initialize = initialize;
		_logger     = logger;
	}

	public async ValueTask Get(Stop<ushort> parameter)
	{
		_logger.LogInformation("Initializing Context...");
		await _initialize.Off(parameter);
		_logger.LogInformation("...Context Initialized!");
		await _previous.Off(parameter);
	}
}