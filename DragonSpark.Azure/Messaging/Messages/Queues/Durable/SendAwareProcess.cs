using DragonSpark.Application;
using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class SendAwareProcess : Appending<Stop<DurableMessageProperties>>, IProcess
{
	public SendAwareProcess(IProcess previous, ISendMessage send) : base(previous, send.Ambient().Out()) {}
}