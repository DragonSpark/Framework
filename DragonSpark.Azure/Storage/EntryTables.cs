using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System.Collections.Generic;

namespace DragonSpark.Azure.Storage;

sealed class EntryTables : ReferenceValueStore<IDictionary<string, string?>, EntryTable>
{
	public static EntryTables Default { get; } = new();

	EntryTables() : base(x => new EntryTable(x.ToTable())) {}
}