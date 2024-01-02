using Azure.Messaging.EventHubs;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Azure.Events.Receive;

sealed class GetEntry : ISelect<EventData, RegistryEntry?>
{
	public static GetEntry Default { get; } = new();

	GetEntry() : this(Entries.Default, Recipient.Default, EventType.Default) {}

	readonly ITable<EntryKey, RegistryEntry> _registry;
	readonly string                          _recipient, _eventType;

	public GetEntry(ITable<EntryKey, RegistryEntry> registry, string recipient, string eventType)
	{
		_registry       = registry;
		_recipient      = recipient;
		_eventType = eventType;
	}

	public RegistryEntry? Get(EventData parameter)
	{
		var recipient = parameter.Properties.TryGetValue(_recipient, out var found)
			                ? found.To<uint>()
			                : default(uint?);
		return parameter.Properties.TryGetValue(_eventType, out var type)
		       &&
		       _registry.TryGet(new(recipient, type.To<string>()), out var element)
			       ? element
			       : null;
	}
}