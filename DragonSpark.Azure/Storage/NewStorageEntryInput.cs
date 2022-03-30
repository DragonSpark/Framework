using System;
using System.IO;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

public readonly record struct NewStorageEntryInput(string Name, string ContentType, ulong Size, DateTime Modified,
                                                   Func<Stream, Task> Write);