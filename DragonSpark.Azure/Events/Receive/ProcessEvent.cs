using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events.Receive;

public sealed class ProcessEvent : IAllocated<ProcessEventArgs>
{
	readonly IOperation<ProcessEventArgs> _process;

	public ProcessEvent(HandleEvent process) => _process = process;

	public Task Get(ProcessEventArgs parameter) => _process.Get(parameter).AsTask();
}