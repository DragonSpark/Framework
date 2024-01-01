using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Azure.Events;

sealed class Entries : ConcurrentTable<EntryKey, RegistryEntry>, IEntries
{
	public static Entries Default { get; } = new();

	Entries() {}
}

public sealed record EntryKey(uint? Recipient, string MessageType)
{
	public EntryKey(string MessageType) : this(null, MessageType) {}
}