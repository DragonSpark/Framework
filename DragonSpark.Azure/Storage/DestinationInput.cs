namespace DragonSpark.Azure.Storage;

public readonly record struct DestinationInput(IStorageEntry Source, string Destination);