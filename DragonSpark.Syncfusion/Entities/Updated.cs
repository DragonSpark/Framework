namespace DragonSpark.SyncfusionRendering.Entities;

public readonly record struct Updated<T>(T Subject, string Action);