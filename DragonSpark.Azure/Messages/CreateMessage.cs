using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;

namespace DragonSpark.Azure.Messages;

sealed class CreateMessage : ISelect<MessageInput, ServiceBusMessage>
{
	public static CreateMessage Default { get; } = new();

	CreateMessage() : this(Time.Default) {}

	readonly ITime _time;

	public CreateMessage(ITime time) => _time = time;

	public ServiceBusMessage Get(MessageInput parameter)
	{
		var (content, visibility, life) = parameter;
		var result = new ServiceBusMessage(content);

		if (visibility is not null)
		{
			result.ScheduledEnqueueTime = _time.Get().Add(visibility.Value);
		}

		if (life is not null)
		{
			result.TimeToLive = life.Value;
		}

		return result;
	}
}