using System;

namespace DragonSpark.Azure.Storage;

public sealed record StorageEntryProperties(Uri Identity, string Name, string ContentType, ulong Size,
                                            DateTimeOffset Created);