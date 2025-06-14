using System.Collections.Generic;

namespace DragonSpark.Azure.Storage;

public sealed class LinkPathVariable : StorageEntryVariable
{
	public LinkPathVariable(IStorageEntry entry) : base("linkPath", entry) {}

	public LinkPathVariable(IDictionary<string, string?> entry) : base("linkPath", entry) {}
}