using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

public readonly record struct WriteInput(string Path, string ContentType, Func<Stream, CancellationToken, Task> Write);