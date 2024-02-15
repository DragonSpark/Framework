using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Model.Operations.Allocated;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events.Receive;

public sealed class ProcessEvents : Allocated<ProcessEventArgs>, IAllocated<ProcessErrorEventArgs>
{
	readonly ProcessError _error;

	public ProcessEvents(AudienceAwareProcessEvent process, ProcessError error) : base(process) => _error = error;

	public Task Get(ProcessErrorEventArgs parameter) => _error.Get(parameter);
}