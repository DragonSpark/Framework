using DragonSpark.Compose;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class OutboxSweeperBackgroundService : BackgroundService
{
	readonly ISweep   _sweep;
	readonly TimeSpan _pollingInterval;

	public OutboxSweeperBackgroundService(ISweep sweep) : this(sweep, TimeSpan.FromSeconds(15)) {}

	public OutboxSweeperBackgroundService(ISweep sweep, TimeSpan pollingInterval)
	{
		_sweep           = sweep;
		_pollingInterval = pollingInterval;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		if (await _sweep.Off(stoppingToken))
		{
			using var timer = new PeriodicTimer(_pollingInterval);

			while (await timer.WaitForNextTickAsync(stoppingToken).Off() && await _sweep.Off(stoppingToken)) {}
		}
	}
}