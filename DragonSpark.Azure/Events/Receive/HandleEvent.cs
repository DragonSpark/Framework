using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events.Receive;

public sealed class HandleEvent : IOperation<ProcessEventArgs>
{
	readonly ISelect<EventData, RegistryEntry?> _entry;
	readonly ProcessEntry                       _process;
	readonly ILogger<HandleEvent>               _logger;

	public HandleEvent(ProcessEntry process, ILogger<HandleEvent> logger) : this(GetEntry.Default, process, logger) {} // TODO

	public HandleEvent(ISelect<EventData, RegistryEntry?> entry, ProcessEntry process, ILogger<HandleEvent> logger)
	{
		_entry       = entry;
		_process     = process;
		_logger = logger;
	}

	public async ValueTask Get(ProcessEventArgs parameter)
	{
		var entry = _entry.Get(parameter.Data);
		if (entry is not null)
		{
			var (key, handlers) = entry;
			_logger.LogInformation("Entry Received! {Type}", key);
			var message = await parameter.Data.Data.Verify().ToObjectAsync(key).ConfigureAwait(false);
			if (message is not null)
			{
				await _process.Await(new(message, handlers));
			}
		}
	}
}