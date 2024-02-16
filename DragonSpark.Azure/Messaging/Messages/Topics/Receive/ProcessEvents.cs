using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Operations.Allocated;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public sealed class ProcessEvents : Allocated<ProcessMessageEventArgs>, IAllocated<ProcessErrorEventArgs>
{
	readonly ProcessError _error;

	public ProcessEvents(AudienceAwareProcessEvent process, ProcessError error) : base(process) => _error = error;

	public Task Get(ProcessErrorEventArgs parameter) => _error.Get(parameter);
}