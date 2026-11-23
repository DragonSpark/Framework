using DragonSpark.Application.Hosting;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class OutboxSweeperBackgroundService : PollingBackgroundService
{
	public OutboxSweeperBackgroundService(OutboxSweeperSettings settings, ISweep action)
		: base(settings.Enabled ? action : EmptySweep.Default, TimeSpan.FromSeconds(15)) {}
}