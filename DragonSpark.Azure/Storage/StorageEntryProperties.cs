using Azure;
using System;
using System.Collections.Generic;

namespace DragonSpark.Azure.Storage;

public sealed record StorageEntryProperties(Uri Identity, string Path, string Name, string ContentType, ulong Size,
											DateTimeOffset Created, DateTimeOffset Modified, ETag Tag,
											IDictionary<string, string?> Metadata)
{
	/*// ReSharper disable once TooManyDependencies
	public StorageEntryProperties(Uri identity, string path, string contentType, ulong size, DateTimeOffset created,
								  DateTimeOffset modified, ETag tag, IDictionary<string, string?> metadata)
		: this(identity, path, System.IO.Path.GetFileName(path), contentType, size, created, modified, tag, metadata) {}*/
}