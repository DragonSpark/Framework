using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;

namespace DragonSpark.Azure.Messaging.Messages;

sealed class CreateMessage : ISelect<MessageInput, ServiceBusMessage>
{
	readonly string? _audience;
	readonly ITime   _time;

	public CreateMessage(string? audience) : this(audience, Time.Default) {}

	public CreateMessage(string? audience, ITime time)
	{
		_audience = audience;
		_time     = time;
	}

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

		if (_audience is not null)
		{
			result.ApplicationProperties[IntendedAudience.Default] = _audience;
		}

		return result;
	}
}