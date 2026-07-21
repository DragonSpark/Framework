using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class OutboxSweeperBackgroundService : BackgroundService
{
	readonly Sweep                                   _sweep;
	readonly TimeSpan                                _pollingInterval;
	
	public OutboxSweeperBackgroundService(Sweep sweep) : this(sweep, TimeSpan.FromSeconds(15)) {}

	public OutboxSweeperBackgroundService(Sweep sweep, TimeSpan pollingInterval)
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