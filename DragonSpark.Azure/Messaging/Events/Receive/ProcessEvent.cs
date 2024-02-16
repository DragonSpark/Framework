using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Events.Receive;

public sealed class ProcessEvent : IAllocated<ProcessEventArgs>
{
	readonly ISelect<EventData, RegistryEntry?> _entry;
	readonly ProcessEntry                       _process;
	readonly IOperation<ProcessEventArgs>       _tracker;
	readonly ILogger<ProcessEvent>              _logger;

	public ProcessEvent(ProcessEntry process, ILogger<ProcessEvent> logger)
		: this(GetEntry.Default, process, CheckpointTracker.Default, logger) {}

	public ProcessEvent(ISelect<EventData, RegistryEntry?> entry, ProcessEntry process,
	                    IOperation<ProcessEventArgs> tracker, ILogger<ProcessEvent> logger)
	{
		_entry   = entry;
		_process = process;
		_tracker = tracker;
		_logger  = logger;
	}

	public async Task Get(ProcessEventArgs parameter)
	{
		var entry = _entry.Get(parameter.Data);
		if (entry is not null)
		{
			var (key, handlers) = entry;
			_logger.LogInformation("Message Received: {Type} {Handlers}", key, handlers.Count);
			var message = await parameter.Data.Data.Verify().ToObjectAsync(key).ConfigureAwait(false);
			if (message is not null)
			{
				await _process.Await(new(message, handlers));
			}
		}

		await _tracker.Await(parameter);
	}
}