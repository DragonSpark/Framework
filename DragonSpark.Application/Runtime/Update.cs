namespace DragonSpark.Application.Runtime;

public readonly record struct Update<T>(T Stored, T Input);