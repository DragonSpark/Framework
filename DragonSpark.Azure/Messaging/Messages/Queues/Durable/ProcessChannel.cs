using DragonSpark.Contracts.Messaging;
using System.Threading.Channels;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class ProcessChannel : Model.Results.Instance<Channel<DurableMessageProperties>>
{
	public static ProcessChannel Default { get; } = new();

	ProcessChannel() : base(Channel.CreateUnbounded<DurableMessageProperties>(new()
	{
		SingleReader = true, SingleWriter = true
	})) {}
}