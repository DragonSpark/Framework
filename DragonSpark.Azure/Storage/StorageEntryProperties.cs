using Azure;

namespace DragonSpark.Azure.Storage;

public sealed record StorageEntryProperties(Uri Identity, string Path, string Name, string ContentType, ulong Size,
											DateTimeOffset Created, DateTimeOffset Modified, ETag Tag,
											IDictionary<string, string?> Metadata, byte[]? Hash);