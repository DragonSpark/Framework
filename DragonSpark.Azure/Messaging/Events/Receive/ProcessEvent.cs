using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Events.Receive;

public sealed class ProcessEvent : IAllocated<ProcessEventArgs>
{
	readonly ISelect<EventData, RegistryEntry?> _entry;
	readonly ProcessEntry                       _process;
	readonly IOperation<ProcessEventArgs>       _tracker;

	public ProcessEvent(ProcessEntry process) : this(GetEntry.Default, process, CheckpointTracker.Default) {}

	public ProcessEvent(ISelect<EventData, RegistryEntry?> entry, ProcessEntry process,
	                    IOperation<ProcessEventArgs> tracker)
	{
		_entry   = entry;
		_process = process;
		_tracker = tracker;
	}

	public async Task Get(ProcessEventArgs parameter)
	{
		var entry = _entry.Get(parameter.Data);
		if (entry is not null)
		{
			var (key, handlers) = entry;
			var message = await parameter.Data.Data.Verify().ToObjectAsync(key).ConfigureAwait(false);
			if (message is not null)
			{
				await _process.Await(new(message, handlers));
			}
		}

		await _tracker.Await(parameter);
	}
}