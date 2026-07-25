using DragonSpark.Application;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class ProcessAwareWriteMessage : IWriteMessage
{
	readonly IStopAware<DurableMessageProperties> _previous;
	readonly CreateProcessNotification            _create;

	[ActivatorUtilitiesConstructor]
	public ProcessAwareWriteMessage(IWriteMessage previous, CreateProcessNotification create) 
		: this(previous.Ambient().Out(), create) {}

	[Candidate(false)]
	public ProcessAwareWriteMessage(IStopAware<DurableMessageProperties> previous, CreateProcessNotification create)
	{
		_previous = previous;
		_create   = create;
	}

	public async ValueTask Get(Stop<DurableMessageProperties> parameter)
	{
		await _create.Off(parameter);
		await _previous.Off(parameter);
	}
}