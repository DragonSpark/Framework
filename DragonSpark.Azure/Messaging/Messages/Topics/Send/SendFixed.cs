using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Send;

public class SendFixed<T> : IStopAware<uint> where T : Message, new()
{
	readonly IStopAware<CreateEventDataInput> _previous;
	readonly T                                _message;

	protected SendFixed(IProducer client) : this(client, new T()) {}

	protected SendFixed(IProducer client, T message) : this(new SendTo<T>(client), message) {}

	protected SendFixed(IStopAware<CreateEventDataInput> previous, T message)
	{
		_previous = previous;
		_message  = message;
	}

	public ValueTask Get(Stop<uint> parameter) => _previous.Get(new(new(parameter, _message), parameter));
}