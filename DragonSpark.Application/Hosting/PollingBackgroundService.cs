using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Stop.Conditions;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Hosting;

public class PollingBackgroundService : BackgroundService
{
	readonly IDepending _action;
	readonly TimeSpan   _pollingInterval;

	protected PollingBackgroundService(IStopAware previous, TimeSpan pollingInterval)
		: this(new Always(previous), pollingInterval) {}

	protected PollingBackgroundService(IDepending action, TimeSpan pollingInterval)
	{
		_action           = action;
		_pollingInterval = pollingInterval;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		if (await _action.Off(stoppingToken))
		{
			using var timer = new PeriodicTimer(_pollingInterval);

			while (await timer.WaitForNextTickAsync(stoppingToken).Off() && await _action.Off(stoppingToken)) {}
		}
	}
}