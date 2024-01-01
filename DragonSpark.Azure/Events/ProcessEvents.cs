using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Model.Operations.Allocated;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events;

public sealed class ProcessEvents : IAllocated<ProcessEventArgs>, IAllocated<ProcessErrorEventArgs>
{
	readonly ProcessEvent _process;
	readonly ProcessError _error;

	public ProcessEvents(ProcessEvent process, ProcessError error)
	{
		_process = process;
		_error   = error;
	}

	public Task Get(ProcessEventArgs parameter) => _process.Get(parameter);

	public Task Get(ProcessErrorEventArgs parameter) => _error.Get(parameter);
}