using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Azure.Events.Receive;

sealed class Entries : ConcurrentTable<EntryKey, RegistryEntry>, IEntries
{
	public static Entries Default { get; } = new();

	Entries() {}
}