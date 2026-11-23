namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

public sealed record OutboxSweeperSettings
{
	public bool Enabled { get; set; } = true;
}