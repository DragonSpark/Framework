namespace DragonSpark.Azure.Storage;

public readonly record struct WriteInput(string Path, string ContentType, Func<Stream, CancellationToken, Task> Write);