using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class Dispatch : IDispatch
{
	readonly string                                _name;
	readonly Channel<DurableMessageProperties> _channel;

	public Dispatch(string name, ServiceBusConfiguration configuration) : this(name, configuration.Audience) {}

	public Dispatch(string name, string? audience) : this($"{name}{audience}", ProcessChannel.Default) {}

	public Dispatch(string name, Channel<DurableMessageProperties> channel)
	{
		_name    = name;
		_channel = channel;
	}

	public ValueTask Get(Stop<MessageProperties> parameter)
	{
		var (((message, identifier), visibility, life), _) = parameter;
		_channel.Writer.TryWrite(new(identifier, message, _name, visibility, life));
		return ValueTask.CompletedTask;
	}
}