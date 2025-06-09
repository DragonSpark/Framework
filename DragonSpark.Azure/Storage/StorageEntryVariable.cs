using DragonSpark.Model.Selection.Stores;
using System.Collections.Generic;

namespace DragonSpark.Azure.Storage;

public class StorageEntryVariable : TableVariable<string, string>
{
	protected StorageEntryVariable(string name, IStorageEntry entry) : this(name, entry.Properties.Metadata) {}

	protected StorageEntryVariable(string name, IDictionary<string, string?> entry)
		: this(name, EntryTables.Default.Get(entry)) {}

	protected StorageEntryVariable(string name, ITable<string, string?> table) : base(name, table) {}
}