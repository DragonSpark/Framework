using Azure.Messaging.EventHubs.Consumer;

namespace DragonSpark.Azure.Messaging.Events;

public sealed class EventHubConfiguration : MessagingConfiguration
{
	public string Group { get; set; } = EventHubConsumerClient.DefaultConsumerGroupName;
}