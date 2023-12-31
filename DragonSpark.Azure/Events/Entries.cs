using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Azure.Events;

sealed class Entries : ConcurrentTable<string, RegistryEntry>, IEntries
{
	public static Entries Default { get; } = new();

	Entries() {}
}