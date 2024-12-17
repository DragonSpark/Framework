using System;
using System.Threading.Tasks;
using DragonSpark.Application.Diagnostics.Initialization;
using DragonSpark.Compose;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.Mobile.Run;

sealed class SynchronousAwareRunApplication(IRunApplication previous, ILogger logger) : IRunApplication
{
	public SynchronousAwareRunApplication(IRunApplication previous)
		: this(previous, DefaultInitializeLog<SynchronousAwareRunApplication>.Default.Get()) {}

	public async Task<Application> Get(InitializeInput parameter)
	{
		try
		{
			return await previous.Await(parameter);
		}
		catch (Exception e)
		{
			logger.LogError(e, "Critical exception occurred while initialization and starting application");
			throw;
		}
	}
}
