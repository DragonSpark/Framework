namespace DragonSpark.Azure.Storage.Uploads;

public readonly record struct CurrentChunkView(ushort Index, ushort Total);