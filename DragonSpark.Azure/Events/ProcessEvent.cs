using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events;

public sealed class ProcessEvent : IAllocated<ProcessEventArgs>
{
	public static ProcessEvent Default { get; } = new();

	ProcessEvent() : this(HandleEvent.Default) {}

	readonly IOperation<ProcessEventArgs> _process;

	public ProcessEvent(IOperation<ProcessEventArgs> process) => _process = process;

	public Task Get(ProcessEventArgs parameter) => _process.Get(parameter).AsTask();
}