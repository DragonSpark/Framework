using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Workers;
using DragonSpark.Contracts.Messaging;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class CreateProcessNotification : Saving<DurableMessageProperties, ProcessNotification>
{
	public CreateProcessNotification(NewProcessNotification compose, Save<ProcessNotification> add)
		: base(compose, add) {}
}