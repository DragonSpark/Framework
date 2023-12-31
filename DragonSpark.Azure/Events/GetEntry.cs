using Azure.Messaging.EventHubs;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Azure.Events;

sealed class GetEntry : ISelect<EventData, RegistryEntry?>
{
	public static GetEntry Default { get; } = new();

	GetEntry() : this(Entries.Default) {}

	readonly ITable<string, RegistryEntry> _registry;

	public GetEntry(ITable<string, RegistryEntry> registry) => _registry = registry;

	public RegistryEntry? Get(EventData parameter)
		=> parameter.Properties.TryGetValue(EventType.Default, out var type)
		   &&
		   _registry.TryGet(type.To<string>(), out var element)
			   ? element
			   : null;
}