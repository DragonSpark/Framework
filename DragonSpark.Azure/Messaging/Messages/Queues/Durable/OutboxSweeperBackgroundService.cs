using DragonSpark.Application.Hosting;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class OutboxSweeperBackgroundService : PollingBackgroundService
{
	public OutboxSweeperBackgroundService(ISweep action) : base(action, TimeSpan.FromSeconds(15)) {}
}