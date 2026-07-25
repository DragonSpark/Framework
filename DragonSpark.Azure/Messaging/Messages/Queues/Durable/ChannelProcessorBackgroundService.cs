using DragonSpark.Compose;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class ChannelProcessorBackgroundService : BackgroundService
{
	readonly ChannelProcessor _process;

	public ChannelProcessorBackgroundService(ChannelProcessor process) => _process = process;

	protected override Task ExecuteAsync(CancellationToken stoppingToken) => _process.Allocate(stoppingToken);
}