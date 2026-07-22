using System.Threading.Channels;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class ProcessChannel : Instance<Channel<DurableMessageProperties>>
{
	public static ProcessChannel Default { get; } = new();

	ProcessChannel() : base(Channel.CreateUnbounded<DurableMessageProperties>(new()
	{
		SingleReader = true, SingleWriter = true
	})) {}
}