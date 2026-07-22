using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Results;
using System.Threading.Channels;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class ProcessReader : Instance<ChannelReader<DurableMessageProperties>>
{
	public ProcessReader(Channel<DurableMessageProperties> instance) : base(instance.Reader) {}
}