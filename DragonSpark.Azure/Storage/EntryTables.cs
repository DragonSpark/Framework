using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Azure.Storage;

sealed class EntryTables : ReferenceValueStore<IDictionary<string, string?>, ITable<string, string?>>, IEntryTable
{
	public static EntryTables Default { get; } = new();

	EntryTables() : base(x => new EntryTable(x.ToTable())) {}
}