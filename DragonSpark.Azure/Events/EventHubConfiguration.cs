using Azure.Messaging.EventHubs.Consumer;

namespace DragonSpark.Azure.Events;

public sealed class EventHubConfiguration
{
	public string Namespace { get; set; } = default!;

	public string Group { get; set; } = EventHubConsumerClient.DefaultConsumerGroupName;
}