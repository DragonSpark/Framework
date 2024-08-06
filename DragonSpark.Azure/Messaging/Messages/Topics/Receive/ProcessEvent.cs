using Azure.Messaging.ServiceBus;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public sealed class ProcessEvent : IAllocated<ProcessMessageEventArgs>
{
	readonly ISelect<ServiceBusReceivedMessage, RegistryEntry?> _entry;
	readonly ProcessEntry                                       _process;

	public ProcessEvent(ProcessEntry process) : this(GetEntry.Default, process) {}

	public ProcessEvent(ISelect<ServiceBusReceivedMessage, RegistryEntry?> entry, ProcessEntry process)
	{
		_entry   = entry;
		_process = process;
	}

	public async Task Get(ProcessMessageEventArgs parameter)
	{
		var entry = _entry.Get(parameter.Message);
		if (entry is not null)
		{
			var (key, handlers) = entry;
			var message = await parameter.Message.Body.Verify().ToObjectAsync(key).Await();
			if (message is not null)
			{
				await _process.Await(new(message, handlers));
			}
		}
	}
}