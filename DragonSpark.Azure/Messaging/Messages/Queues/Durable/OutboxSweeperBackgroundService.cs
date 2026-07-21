using DragonSpark.Compose;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class OutboxSweeperBackgroundService : BackgroundService
{
	readonly Sweep                                   _sweep;
	readonly TimeSpan                                _pollingInterval;
	readonly ILogger<OutboxSweeperBackgroundService> _logger;

	public OutboxSweeperBackgroundService(Sweep sweep, ILogger<OutboxSweeperBackgroundService> logger)
		: this(sweep, TimeSpan.FromSeconds(15), logger) {}

	public OutboxSweeperBackgroundService(Sweep sweep, TimeSpan pollingInterval,
	                                      ILogger<OutboxSweeperBackgroundService> logger)
	{
		_sweep           = sweep;
		_pollingInterval = pollingInterval;
		_logger          = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using var timer = new PeriodicTimer(_pollingInterval);

		while (await timer.WaitForNextTickAsync(stoppingToken).Off())
		{
			try
			{
				await _sweep.Off(stoppingToken);
			}
			catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
			{
				break;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred during outbox database sweep");
			}
		}
	}
}