using Azure.Messaging.ServiceBus;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class ComposeMessage : ISelect<MessageProperties, ServiceBusMessage>
{
	public static ComposeMessage Default { get; } = new();

	ComposeMessage() : this(Time.Default) {}

	readonly ITime _time;

	public ComposeMessage(ITime time) => _time = time;

	public ServiceBusMessage Get(MessageProperties parameter)
	{
		var ((content, identifier), visibility, life) = parameter;
		var result = new ServiceBusMessage(content);

		if (identifier is not null)
		{
			result.MessageId = identifier.ToString();
		}

		if (visibility is not null)
		{
			result.ScheduledEnqueueTime = _time.Get() + visibility.Value;
		}

		if (life is not null)
		{
			result.TimeToLive = life.Value;
		}

		return result;
	}
}