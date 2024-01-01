using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events;

public sealed class HandleEvent : IOperation<ProcessEventArgs>
{
	readonly ISelect<EventData, RegistryEntry?> _entry;
	readonly ProcessEntry                       _process;

	public HandleEvent(ProcessEntry process) : this(GetEntry.Default, process) {}

	public HandleEvent(ISelect<EventData, RegistryEntry?> entry, ProcessEntry process)
	{
		_entry   = entry;
		_process = process;
	}

	public async ValueTask Get(ProcessEventArgs parameter)
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
	}
}