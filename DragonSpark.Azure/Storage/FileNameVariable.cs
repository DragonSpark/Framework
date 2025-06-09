using System.Collections.Generic;

namespace DragonSpark.Azure.Storage;

public sealed class FileNameVariable : StorageEntryVariable
{
	public FileNameVariable(IStorageEntry entry) : base("fileName", entry) {}

	public FileNameVariable(IDictionary<string, string?> entry) : base("fileName", entry) {}
}