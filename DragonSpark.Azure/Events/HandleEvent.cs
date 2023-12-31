using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events;

sealed class HandleEvent : IOperation<ProcessEventArgs>
{
	public static HandleEvent Default { get; } = new();

	HandleEvent() : this(GetEntry.Default) {}

	readonly ISelect<EventData, RegistryEntry?> _entry;

	public HandleEvent(ISelect<EventData, RegistryEntry?> entry) => _entry = entry;

	public async ValueTask Get(ProcessEventArgs parameter)
	{
		var entry = _entry.Get(parameter.Data);
		if (entry is not null)
		{
			var (key, handlers) = entry;
			var message = await parameter.Data.Data.Verify().ToObjectAsync(key).ConfigureAwait(false);
			if (message is not null)
			{
				using var lease = handlers.AsValueEnumerable().ToArray(ArrayPool<IOperation<object>>.Shared);
				foreach (var operation in lease)
				{
					await operation.Await(message);
				}
			}
		}
	}
}