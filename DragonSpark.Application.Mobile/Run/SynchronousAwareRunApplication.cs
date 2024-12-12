using DragonSpark.Application.Diagnostics.Initialization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Mobile.Run;

sealed class SynchronousAwareRunApplication : IRunApplication
{
	readonly IRunApplication _previous;
	readonly ILogger         _logger;

	public SynchronousAwareRunApplication(IRunApplication previous)
		: this(previous, DefaultInitializeLog<SynchronousAwareRunApplication>.Default.Get()) {}

	public SynchronousAwareRunApplication(IRunApplication previous, ILogger logger)
	{
		_previous = previous;
		_logger   = logger;
	}

	public Task<Application> Get(InitializeInput parameter)
	{
		try
		{
			return _previous.Get(parameter);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Critical exception occurred while initialization and starting application");
			throw;
		}
	}
}