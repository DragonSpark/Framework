using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Results;
using System.Threading.Channels;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class ProcessWriter : Instance<ChannelWriter<DurableMessageProperties>>
{
	public ProcessWriter(Channel<DurableMessageProperties> instance) : base(instance.Writer) {}
}